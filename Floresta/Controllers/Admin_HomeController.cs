using Floresta.Models;
using Floresta.Services;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace Floresta.Controllers
{
    [Authorize(Roles = "admin")]
    public class Admin_HomeController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private FlorestaDbContext _context;

        public Admin_HomeController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            FlorestaDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var model = new ShowUserViewModel
                    {
                        Name = user.Name,
                        Surname = user.UserSurname,
                        Email = user.Email
                    };

                    return View(model);
                }
            }
            return View();
        }

        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _context.Questions.Include(c => c.User).ToListAsync();
            return View(questions);
        }

        public IActionResult AnswerQuestion()
        {
            return View();
        }
  
        public IActionResult PurchasesDiagram()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AnswerQuestion(int id, SendEmailViewModel model)
        {
            Question question = _context.Questions.FirstOrDefault(x => id == x.Id);
            var user = _context.Users.FirstOrDefault(x => question.UserId == x.Id);
            EmailService emailService = new EmailService();

            await emailService.SendEmailAsync(user.Email, "The answer for your question",
                $"Your question was \"{question.QuestionText}\"\n\nThe official answer for your question:\n\n{model.Message}");
            question.IsAnswered = true;
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetQuestions");
        }


        public IActionResult Purchases()
        {
            var purchases = _context
                .Payments
                .Include(u => u.User)
                .Include(m => m.Marker)
                .Include(s => s.Seedling)
                .ToList();

            return View(purchases);
        }

        
        [HttpPost]
        public async Task<IActionResult> Purchases(int? id)
        {
            if (id != null)
            {
                var purchase = _context.Payments.FirstOrDefault(x => x.Id == id);
                var user = _context.Users.FirstOrDefault(x => x.Id == purchase.UserId);
                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(user.Email, "Congratulations!!!",
                    $"Dear {user.Name} {user.UserSurname}, your purchase was successfully confirmed!\nYour desire to save the world is bigger than our graditude to you!\nFollow our updates to be aware of everything!");
                purchase.IsPaymentSucceded = true;
                _context.Update(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction("Purchases");
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeclinePurchase(int? id)
        {
            if (id != null)
            {
                var purchase = _context.Payments.FirstOrDefault(x => x.Id == id);
                var user = _context.Users.FirstOrDefault(x => x.Id == purchase.UserId);
                var seedling = _context.Seedlings.FirstOrDefault(x => x.Id == purchase.SeedlingId);
                var marker = _context.Markers.FirstOrDefault(x => x.Id == purchase.MarkerId);

                EmailService emailService = new EmailService();
                await emailService.SendEmailAsync(user.Email, "Payment Faliled",
                    $"Dear {user.Name} {user.UserSurname}, unfortunately, your payment was not successfull. Contact our support to get more information.");
                
                seedling.Amount += purchase.PurchasedAmount;
                _context.Update(seedling);

                marker.PlantCount += purchase.PurchasedAmount;
                marker.isPlantingFinished = false;
                _context.Update(marker);
                
                purchase.IsPaymentFailed = true;
                
                _context.Update(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction("Purchases");
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> GetTeamParticipants()
        {
            var participants = await _context
                .Users
                .Where(u => u.IsClaimingForTeamParticipating)
                .ToListAsync();
            return View(participants);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmParticipating(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "moderator");
                user.IsTeamParticipant = true;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(user.Email, "Participating status",
                    $"Dear {user.Name} {user.UserSurname}, you are now officially a participant of Floresta Team!<br /> " +
                    $"Let's make make this world cleaner with fresh oxygen from our trees!");
                return RedirectToAction("GetTeamParticipants");
            }
            else
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeclineParticipating(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                user.IsClaimingForTeamParticipating = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(user.Email, "Participating status",
                    $"Dear {user.Name} {user.UserSurname}, unfortunately, you have not become a participant of Floresta Team, " +
                    $"but you should still keep up with a thought that you can make this world better!");
                return RedirectToAction("GetTeamParticipants");
            }
            else
                return NotFound();
        }
    }
}
