using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Models
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public string Image { get;set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
