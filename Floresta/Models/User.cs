using Microsoft.AspNetCore.Identity;

namespace Floresta.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string UserSurname { get; set; }
    }
}
