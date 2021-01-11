using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.ViewModels
{
    public class QuestionTopicViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Topic { get; set; }
    }
}
