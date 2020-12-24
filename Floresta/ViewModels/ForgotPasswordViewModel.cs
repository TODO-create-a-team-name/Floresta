
using System.ComponentModel.DataAnnotations;

namespace Floresta.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
