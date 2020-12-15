using Floresta.Models;

namespace Floresta.ViewModels
{
    public class PaymentViewModel
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public Seedling Seedling { get; set; }
    }
}
