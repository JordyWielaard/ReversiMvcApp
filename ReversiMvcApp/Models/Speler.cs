using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string SpelerRol { get; set; }

        [NotMapped]
        public List<SelectListItem> Rollen { get; } = new List<SelectListItem>
        {
            new SelectListItem{ Value = "beheerder", Text = "beheerder" },
            new SelectListItem{ Value = "mediator", Text = "mediator" },
            new SelectListItem{ Value = "speler", Text = "speler" }
        };

        public Speler()
        {
        }

    }
}
