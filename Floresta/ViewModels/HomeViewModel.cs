using System;
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class HomeViewModel
    { 
        [Required]
        public string Question { get; set; }

        [Required(ErrorMessage ="Введіть, будь ласка, номер телефону")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Некоректний номер телефону")]
        public string PhoneNumber { get; set; }
    }
}
