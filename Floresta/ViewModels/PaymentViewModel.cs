using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Floresta.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        public int MarkerId { get;set; }

        public string Title { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }
        public int PlantCount { get; set; }

       public int SeedlingId { get; set; }
        public List<SelectListItem> Seedlings { get; set; }
    }
}
