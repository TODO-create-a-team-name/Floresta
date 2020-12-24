using Floresta.Models;
using Floresta.Services;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
                var seedling = _context.Seedlings.FirstOrDefault(x => x.Id == purchase.SeedlingId);
                var marker = _context.Markers.FirstOrDefault(x => x.Id == purchase.MarkerId);
                var user = _context.Users.FirstOrDefault(x => x.Id == purchase.UserId);
                EmailService emailService = new EmailService();
                seedling.Amount += purchase.PurchasedAmount;
                _context.Update(seedling);
                marker.PlantCount += purchase.PurchasedAmount;
                _context.Update(marker);
                await emailService.SendEmailAsync(user.Email, "Payment Faliled",
                    $"Dear {user.Name} {user.UserSurname}, unfortunately, your payment was not successfull. Contact our support to get more information.");
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
    }
}
