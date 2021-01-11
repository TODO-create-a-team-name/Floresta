using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.Models
{
    public class QuestionTopic
    {
        public int Id { get; set; }
        [Required]
        public string Topic { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
