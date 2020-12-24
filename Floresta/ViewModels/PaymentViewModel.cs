using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Title filed is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The Lat filed is required")]
        public string Lat { get; set; }
        [Required(ErrorMessage = "The Lng filed is required")]
        public string Lng { get; set; }
        public int MarkerId { get;set; }
        [Required(ErrorMessage = "The PlantCount filed is required")]
        public int PlantCount { get; set; }
        public int SeedlingId { get; set; }
        public List<SelectListItem> Seedlings { get; set; }
    }
}
