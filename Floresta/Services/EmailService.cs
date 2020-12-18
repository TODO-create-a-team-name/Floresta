using Floresta.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;

using System.Threading.Tasks;

namespace Floresta.Services
{
    public class EmailService
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public EmailService(UserManager<User> userManager = null, SignInManager<User> signInManager = null)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Floresta Administration", "florestaofficial200@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, false);
                await client.AuthenticateAsync("florestaofficial200@gmail.com", "floresta34356543");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
