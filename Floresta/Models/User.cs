using Microsoft.AspNetCore.Identity;
using MimeKit.Cryptography;
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
        public bool IsTeamParticipant { get; set; } = false;
        public bool IsClaimingForTeamParticipating { get; set; } = false;
        public List<Question> Questions { get; set; } = new List<Question>();
        public List<Payment> Payments { get; set; } = new List<Payment>();
    }
}
