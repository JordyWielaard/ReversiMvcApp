using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReversiMvcApp.Data;
using ReversiMvcApp.Models;
using ReversiMvcApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace ReversiMvcApp.Controllers
{



    [Authorize(Roles = "Beheerder,Mediator,Speler")]
    public class SpelController : Controller
    {
        private readonly ReversiDbContext _reversiDbContext;
        private readonly Helper _helper = new Helper();
        private HttpClient _client;

        public SpelController(ILogger<SpelController> logger, ReversiDbContext reversiDbContext)
        {
            _reversiDbContext = reversiDbContext;
            _client = _helper.ClientInit();
        }
        
       


        // GET: SpelController
        public ActionResult Index()
        {           
            if (_helper.CheckVoorSpel(this.User) != null)
            {
                return RedirectToAction("Details", "Spel", new { id = _helper.CheckVoorSpel(this.User).Token });
            }
            else
            {
                string apiUri = "api/spel";
                HttpResponseMessage responseMessage = _client.GetAsync(apiUri).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                    var responeList = JsonConvert.DeserializeObject<List<Spel>>(responseBody);
                    return View(responeList);
                }
            }

            return View();
        }

        // GET: SpelController/Details/5
        public ActionResult Details(string id)
        {
            var currentUserID = _helper.CurrentUserId(this.User);
            //Ophalen spel
            string apiUri = "api/spel/" + id;
            HttpResponseMessage responseMessage = _client.GetAsync(apiUri).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                var respone = JsonConvert.DeserializeObject<Spel>(responseBody);
                respone.CurrentUser = currentUserID;
                //als het spel al is afgelopen wordt de speler terug verwezen naar de lijst met spellen
                if (respone.Afgelopen)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (respone.Speler2Token == ""  && respone.Speler1Token != currentUserID)
                {
                    string GameApiUri = "api/spel/spelertoevoegen";
                    
                    var responseUpdate = _client.PutAsJsonAsync(GameApiUri, new JoinGame() { SpelerToken = currentUserID , SpelToken = respone.Token } ).Result;
                    return View(respone);
                }
                return View(respone);
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: SpelController/Create
        public ActionResult Create()
        {
            if (_helper.CheckVoorSpel(this.User) != null)
            {
                return RedirectToAction("Details", "Spel", new { id = _helper.CheckVoorSpel(this.User).Token });
            }
            return View();
        }

        // POST: SpelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Omschrijving")] Spel spel)
        {
            ClaimsPrincipal currentUser = this.User;
            if (_helper.CheckVoorSpel(this.User) != null)
            {
                return RedirectToAction("Details", "Spel", new { id = _helper.CheckVoorSpel(this.User).Token });
            }
            else if (ModelState.IsValid)
            {
                spel.Speler1Token = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                string GameApiUri = "api/spel";
                var response = _client.PostAsJsonAsync(GameApiUri, spel).Result;
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: SpelController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SpelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: SpelController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SpelController/Delete/5
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
