using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
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
        private readonly ILogger<HomeController> _logger;
        private readonly ReversiDbContext _reversiDbContext;


        private string requestUri = "https://localhost:44346";
        private HttpClient client;

        public HomeController(ILogger<HomeController> logger, ReversiDbContext reversiDbContext)
        {
            _logger = logger;
            _reversiDbContext = reversiDbContext;



            client = new HttpClient();
            client.BaseAddress = new Uri(requestUri);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResult Index()
        {
            

            if (User.Identity.IsAuthenticated)
            {
                //check if player has record if not exists create
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (!_reversiDbContext.Spelers.Any(s => s.Guid == currentUserID))
                {
                    _reversiDbContext.Spelers.Add(new Speler() { Guid = currentUserID, Naam = currentUser.Identity.Name });
                    _reversiDbContext.SaveChanges();
                }

                if (CheckForGame() != null)
                {
                    return RedirectToAction("Details", "Spel", new { id = CheckForGame().Token });
                }
                return RedirectToAction("Index", "Spel");
            }
            return View();
        }

        public Spel CheckForGame()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            string apiUri = "api/spel/speler/" + currentUserID;
            HttpResponseMessage responseMessage = client.GetAsync(apiUri).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                var respone = JsonConvert.DeserializeObject<Spel>(responseBody);
                if (respone != null)
                {
                    if (!respone.Finished)
                    {
                        return respone;
                    }
                }              
            }
            return null;
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
