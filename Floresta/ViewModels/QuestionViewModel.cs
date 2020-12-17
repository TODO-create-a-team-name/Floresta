

using Floresta.Models;
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class QuestionViewModel
    { 
        [Required]
        public string Question { get; set; }
    }
}
