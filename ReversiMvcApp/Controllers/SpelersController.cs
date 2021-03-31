using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;

namespace ReversiMvcApp.Controllers
{
    [Authorize(Roles = "Beheerder,Mediator,Speler")]
    public class SpelersController : Controller
    {
        private readonly ReversiDbContext _context;
        private readonly Helper _helper = new Helper();
        private HttpClient _client;

        public SpelersController(ReversiDbContext context)
        {
            _context = context;
            _client = _helper.ClientInit();
        }

        // GET: Spelers
        public async Task<IActionResult> Index()
        {
            var spelers = await _context.Spelers.ToListAsync();
            foreach (var speler in spelers)
            {
                speler.AantalGelijk = 0;
                speler.AantalGewonnen = 0;
                speler.AantalVerloren = 0;
            }
            string apiUri = "api/Spel/AfgelopenSpellen";
            List<Speler> spelerupdate = new List<Speler>();
            HttpResponseMessage responseMessage = await _client.GetAsync(apiUri);
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = await responseMessage.Content.ReadAsStringAsync();
                List <Spel> respone = JsonConvert.DeserializeObject<List<Spel>>(responseBody);
                foreach (var spel in respone)
                {
                    if (spel.Winnaar != "Gelijkspel")
                    {
                        var winnaar = spelers.Find(s => s.Guid == spel.Winnaar);
                        if (spel.Winnaar == spel.Speler1Token)
                        {
                            var verliezer = spelers.Find(s => s.Guid == spel.Speler2Token);
                            verliezer.AantalVerloren += 1;
                        }
                        else if (spel.Winnaar == spel.Speler2Token)
                        {
                            var verliezer = spelers.Find(s => s.Guid == spel.Speler1Token);
                            verliezer.AantalVerloren += 1;
                        }
                        winnaar.AantalGewonnen += 1;                    
                    }
                    else
                    {
                        var speler1 = spelers.Find(s => s.Guid == spel.Speler1Token);
                        var speler2 = spelers.Find(s => s.Guid == spel.Speler2Token);
                        speler1.AantalGelijk += 1;
                        speler2.AantalGelijk += 1;
                    }
                }
                _context.Spelers.UpdateRange(spelers);
                await _context.SaveChangesAsync();
            }
            return View(spelers.OrderByDescending(s => s.AantalGewonnen).ThenByDescending(s => s.AantalGelijk).ThenBy(s => s.AantalVerloren));
        }

        // GET: Spelers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        // GET: Spelers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Spelers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(speler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        // GET: Spelers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers.FindAsync(id);
            if (speler == null)
            {
                return NotFound();
            }
            return View(speler);
        }

        // POST: Spelers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk")] Speler speler)
        {
            if (id != speler.Guid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(speler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpelerExists(speler.Guid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(speler);
        }

        // GET: Spelers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers
                .FirstOrDefaultAsync(m => m.Guid == id);
            if (speler == null)
            {
                return NotFound();
            }

            return View(speler);
        }

        // POST: Spelers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var speler = await _context.Spelers.FindAsync(id);
            _context.Spelers.Remove(speler);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SpelerExists(string id)
        {
            return _context.Spelers.Any(e => e.Guid == id);
        }
    }
}
