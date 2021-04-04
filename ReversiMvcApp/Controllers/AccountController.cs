using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;
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
        private readonly ILogger<AccountController> _logger;
        private readonly Helper _helper = new Helper();


        private HttpClient _client;

        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ReversiDbContext context, SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
            _client = _helper.ClientInit();
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

                    //Haal alle rolen op van gebruiker
                    var roles = await _userManager.GetRolesAsync(user);
                    //Verwijder alle rolen van gebruiker
                    await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                    //Voeg nieuwe rol toe aan gebruiker
                    await _userManager.AddToRoleAsync(user, speler.SpelerRol);

                    var spelerupdate = await _context.Spelers.FirstOrDefaultAsync(s => s.Guid == speler.Guid);
                    spelerupdate.SpelerRol = speler.SpelerRol;
                    spelerupdate.AantalVerloren = speler.AantalVerloren;
                    spelerupdate.AantalGewonnen = speler.AantalGewonnen;
                    spelerupdate.AantalGelijk = speler.AantalGelijk;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Gebruiker {this.User.Identity.Name} heeft de rol van {speler.Naam} aangepast vanaf IP {HttpContext.Connection.RemoteIpAddress} om {DateTime.Now}");
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
            
            //Delete spellen van speler uit api db
            string apiUri = "api/spel/" + speler.Guid + "";
            HttpResponseMessage responseMessage = await _client.DeleteAsync(apiUri);
            if (responseMessage.IsSuccessStatusCode)
            {
                _context.Spelers.Remove(speler);
                await _context.SaveChangesAsync();
                //Haal alle rolen op van gebruiker
                var user = await _userManager.FindByIdAsync(id);
                //Verwijder alle rolen van gebruiker
                var roles = await _userManager.GetRolesAsync(user);
                //Remove alle rolen van user
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                await _userManager.DeleteAsync(user);
                _logger.LogInformation($"Gebruiker {this.User.Identity.Name} heeft account van {speler.Naam} verwijderd vanaf IP {HttpContext.Connection.RemoteIpAddress} om {DateTime.Now}");
            }

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> ChangePassword(string guid)
        {
            if (guid == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers.FirstOrDefaultAsync(m => m.Guid == guid);
            if (speler == null)
            {
                return NotFound();
            }

            ChangePassword changePassword = new ChangePassword()
            {
                Guid = guid
            };
            return View(changePassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePassword changePassword)
        {
            var speler = await _userManager.FindByIdAsync(changePassword.Guid);
            //Check modelstate
            if (ModelState.IsValid)
            {
                //Genereer een wachtwoord reset token voor de user
                var token = await _userManager.GeneratePasswordResetTokenAsync(speler);
                //verander het wachtwoord van de speler
                IdentityResult passwordChangeResult = await _userManager.ResetPasswordAsync(speler, token, changePassword.NewPassword);
                //Check of wachtwoord succesvol is aangepast
                if (passwordChangeResult.Succeeded)
                {
                    
                    _logger.LogInformation($"Gebruiker {this.User.Identity.Name} heeft het wachtwoord aangepast van {speler.Email} vanaf IP {HttpContext.Connection.RemoteIpAddress} om {DateTime.Now}");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogInformation($"Gebruiker {this.User.Identity.Name} heeft geprobeerd het wachtwoord aangepast van {speler.Email} vanaf IP {HttpContext.Connection.RemoteIpAddress} om {DateTime.Now}");
                    ModelState.AddModelError("NewPassword", "Invalid Password");
                    ModelState.AddModelError("NewPasswordConfirm", "Invalid Password");
                    return View();
                }
            }
            else
            {
                return View();
            }
            
        }




        [HttpGet]
        public async Task<IActionResult> ResetAuthenticator(string guid)
        {
            if (guid == null)
            {
                return NotFound();
            }

            var speler = await _context.Spelers.FirstOrDefaultAsync(m => m.Guid == guid);
            if (speler == null)
            {
                return NotFound();
            }

            ResetAuthenticator resetAuthenticator = new ResetAuthenticator()
            {
                Guid = guid
            };
            return View(resetAuthenticator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetAuthenticator(ResetAuthenticator resetAuthenticator)
        {
            var speler = await _userManager.FindByIdAsync(resetAuthenticator.Guid);
            //Kijk of gebruiker 2FA heeft
            if (speler.TwoFactorEnabled)
            {
                if (ModelState.IsValid)
                {
                    //Verwijder 2FA van gebruiker
                    await _userManager.ResetAuthenticatorKeyAsync(speler);
                    await _userManager.SetTwoFactorEnabledAsync(speler, false);
                }
                return View();
            }
            else
            {
                return View();
            }
        }
    }
}