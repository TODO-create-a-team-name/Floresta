using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.Models
{
    public class Marker
    { 
        public int Id { get; set; }
        [Required(ErrorMessage = "The Title filed is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The Lat filed is required")]
        public string Lat { get; set; }
        [Required(ErrorMessage = "The Lng filed is required")]
        public string Lng { get; set; }
        public int PlantCount { get; set; }
        public bool isPlantingFinished { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
