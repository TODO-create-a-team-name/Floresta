using Microsoft.AspNetCore.Identity;

namespace Floresta.Models
{
    public class User : IdentityUser
    {
        public string UserSurname { get; set; }
    }
}
