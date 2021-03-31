using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ReversiDbContext _reversiDbContext;

        private readonly Helper _helper = new Helper();

        public HomeController( ReversiDbContext reversiDbContext)
        {
            _reversiDbContext = reversiDbContext;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                //check if player has record if not exists create
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!_reversiDbContext.Spelers.Any(s => s.Guid == currentUserID))
                {
                    await _reversiDbContext.Spelers.AddAsync(new Speler() { Guid = currentUserID, Naam = currentUser.Identity.Name, SpelerRol = "Speler" });
                    await _reversiDbContext.SaveChangesAsync();
                }

                if (_helper.CheckVoorSpel(this.User) != null)
                {
                    return RedirectToAction("Details", "Spel", new { id = _helper.CheckVoorSpel(this.User).Token });
                }
                return RedirectToAction("Index", "Spel");
            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
