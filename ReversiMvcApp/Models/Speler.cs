using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiMvcApp.Models
{
    public class Speler
    {
        [Key]
        public string Guid { get; set; }
        public string Naam { get; set; }
        public int AantalGewonnen { get; set; }
        public int AantalVerloren { get; set; }
        public int AantalGelijk { get; set; }
    }
}
