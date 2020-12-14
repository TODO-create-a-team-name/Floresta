using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Models
{
    public class Marker
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
