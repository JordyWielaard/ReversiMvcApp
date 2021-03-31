using Newtonsoft.Json;
using ReversiMvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ReversiMvcApp.Services
{
    public class Helper
    {


        public HttpClient ClientInit()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44346");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        public Spel CheckVoorSpel(ClaimsPrincipal user)
        {
            if (user.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = user;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                string apiUri = "api/spel/speler/" + currentUserID;
                HttpResponseMessage responseMessage = ClientInit().GetAsync(apiUri).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseBody = responseMessage.Content.ReadAsStringAsync().Result;
                    var respone = JsonConvert.DeserializeObject<Spel>(responseBody);
                    if (respone != null)
                    {
                        if (!respone.Afgelopen)
                        {
                            return respone;
                        }
                    }
                }
            }
            return null;
        }

        public string CurrentUserId(ClaimsPrincipal user)
        {
            ClaimsPrincipal currentUser = user;
            return currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}
