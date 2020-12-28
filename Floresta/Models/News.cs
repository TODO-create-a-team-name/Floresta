using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Models
{
    public class News
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "The Title filed is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "The Context filed is required")]
        public string Content { get; set; }
        public string Image { get;set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}