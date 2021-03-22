using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    [Authorize(Roles = "Beheerder,Mediator")]
    public class AccountController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ReversiDbContext _context;

        private string requestUri = "https://localhost:44346";
        private HttpClient client;
        private HttpResponseMessage responseMessage;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ReversiDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

            client = new HttpClient();
            client.BaseAddress = new Uri(requestUri);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        private bool SpelerExists(string id)
        {
            return _context.Spelers.Any(e => e.Guid == id);
        }


        // GET: AccountController
        public async Task<ActionResult> Index()
        {
            return View(await _context.Spelers.ToListAsync());
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [Authorize(Roles = "Beheerder")]
        // GET: AccountController/Edit/5
        // GET: Spelers/Edit/5
        [Authorize(Roles = "Beheerder")]
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

        // POST: Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Beheerder")]
        public async Task<ActionResult> EditAsync(string id, [Bind("Guid,Naam,AantalGewonnen,AantalVerloren,AantalGelijk,SpelerRol")] Speler speler)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);

                    //Get all roles from user
                    var roles = await _userManager.GetRolesAsync(user);
                    //Remove all roles from user
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    //Add new role to user
                    await _userManager.AddToRoleAsync(user, speler.SpelerRol);

                    var spelerupdate = await _context.Spelers.FirstOrDefaultAsync(s => s.Guid == speler.Guid);
                    spelerupdate.SpelerRol = speler.SpelerRol;
                    spelerupdate.AantalVerloren = speler.AantalVerloren;
                    spelerupdate.AantalGewonnen = speler.AantalGewonnen;
                    spelerupdate.AantalGelijk = speler.AantalGelijk;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
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
            
            string apiUri = "api/spel/" + speler.Guid + "";
            HttpResponseMessage responseMessage = await client.DeleteAsync(apiUri);
            if (responseMessage.IsSuccessStatusCode)
            {
                _context.Spelers.Remove(speler);
                await _context.SaveChangesAsync();

                var user = await _userManager.FindByIdAsync(id);
                //Get all roles from user
                var roles = await _userManager.GetRolesAsync(user);
                //Remove all roles from user
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}