using System;
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class HomeViewModel
    { 
        [Required]
        public string Question { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}
