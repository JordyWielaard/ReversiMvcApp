using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Text;

namespace ReversiMvcApp.Controllers
{
    [Authorize]
    public class SpelController : Controller
    {
        private string requestUri = "https://localhost:44346";
        private HttpClient client;

        public SpelController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(requestUri);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public Spel CheckForGame()
        {
            if (User.Identity.IsAuthenticated)
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
            }
            return null;
        }


        // GET: SpelController
        public ActionResult Index()
        {
            if (CheckForGame() != null)
            {
                return RedirectToAction("Details", "Spel", new { id = CheckForGame().Token });
            }
            else
            {
                string apiUri = "api/spel";
                HttpResponseMessage responseMessage = client.GetAsync(apiUri).Result;
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

            string apiUri = "api/spel/" + id;
            HttpResponseMessage responseMessage = client.GetAsync(apiUri).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                var respone = JsonConvert.DeserializeObject<Spel>(responseBody);
                return View(respone);
            }
            return View();
        }

        // GET: SpelController/Create
        public ActionResult Create()
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (CheckForGame() != null)
            {
                return RedirectToAction("Details", "Spel", new { id = CheckForGame().Token });
            }
            return View();
        }

        // POST: SpelController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Omschrijving")] Spel spel)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (CheckForGame() != null)
            {
                return RedirectToAction("Details", "Spel", new { id = CheckForGame().Token });
            }
            else if (ModelState.IsValid)
            {
                spel.Speler1Token = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                string GameApiUri = "api/spel";
                var response = client.PostAsJsonAsync(GameApiUri, spel).Result;
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
