using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Models
{
    public class Seedling
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Height { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
    }
}
