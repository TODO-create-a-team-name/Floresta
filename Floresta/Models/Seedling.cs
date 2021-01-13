using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.Models
{
    public class Seedling
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Amount { get; set; }
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
