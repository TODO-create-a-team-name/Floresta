using Floresta.Models;

namespace Floresta.ViewModels
{
    public class PaymentViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Lat { get; set; }

        public string Lng { get; set; }
        public int PlantCount { get; set; }

       // public string Email { get; set; }
        public string Seedling { get; set; }
    }
}
