using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class HomeViewModel
    { 
        [Required(ErrorMessage ="Напишіть, будь ласка, ваше питання")]
        public string Question { get; set; }
        [Required(ErrorMessage ="Будь ласка, виберіть тему для питання/звернення")]
        public int TopicId { get; set; }

        public IEnumerable<SelectListItem> Topics { get; set; }

        [Required(ErrorMessage ="Введіть, будь ласка, номер телефону")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Некоректний номер телефону")]
        public string PhoneNumber { get; set; }
    }
}
