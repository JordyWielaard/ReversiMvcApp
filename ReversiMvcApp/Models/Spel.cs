using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class Spel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Speler1Token { get; set; }
        public string Speler2Token { get; set; }
        public string Omschrijving { get; set; }
        public string Winnaar { get; set; }
        public bool Finished { get; set; }
        [NotMapped]
        public string Bord { get; set; }

        public Spel()
        {

        }

        public Spel(string speler1Token, string omschrijving)
        {
            Speler1Token = speler1Token;
            Omschrijving = omschrijving;
        }
    }
}
