using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Floresta.Models
{
    public class User : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserSurname { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
