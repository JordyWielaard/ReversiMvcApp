using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ReversiDbContext _context;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ReversiDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        private bool SpelerExists(string id)
        {
            return _context.Spelers.Any(e => e.Guid == id);
        }
        [Authorize(Roles = "beheerder,mediator")]
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

        // GET: AccountController/Edit/5
        // GET: Spelers/Edit/5
        [Authorize(Roles = "beheerder")]
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
        [Authorize(Roles = "beheerder")]
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

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
