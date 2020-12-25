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
    public class HomeController : Controller
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;
        private FlorestaDbContext _context;

        public HomeController(SignInManager<User> signInManager, UserManager<User> userManager, FlorestaDbContext context)
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
                if(user != null) 
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

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }

        public IActionResult AskQuestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AskQuestion(QuestionViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                Question question = new Question
                {
                    QuestionText = model.Question
                };
                _context.Questions.Add(question);
                user.Questions.Add(question);
                _context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login", "Account");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _context.Questions.Include(c => c.User).ToListAsync();
            return View(questions);
        }

        [Authorize(Roles = "admin")]
        public IActionResult AnswerQuestion()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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
                return RedirectToAction("Purchases", "Home");
            }
            else
            {
                return NotFound();
            }            
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeclinePurchase(int? id)
        {
            if(id != null)
            {

                var purchase = _context.Payments.FirstOrDefault(x => x.Id == id);
                var seedling = _context.Seedlings.FirstOrDefault(x => x.Id == purchase.SeedlingId);
                var marker = _context.Markers.FirstOrDefault(x => x.Id == purchase.MarkerId);
                var user = _context.Users.FirstOrDefault(x => x.Id == purchase.UserId);
                EmailService emailService = new EmailService();
                seedling.Amount += purchase.PurchasedAmount;
                _context.Update(seedling);
                marker.PlantCount += purchase.PurchasedAmount;
                marker.isPlantingFinished = false;
                _context.Update(marker);
                await emailService.SendEmailAsync(user.Email, "Payment Faliled",
                    $"Dear {user.Name} {user.UserSurname}, unfortunately, your payment was not successfull. Contact our support to get more information.");
                purchase.IsPaymentFailed = true;
                _context.Update(purchase);
                await _context.SaveChangesAsync();
                return RedirectToAction("Purchases", "Home");
            }
            else
            {
                return NotFound();
            }
        }

        public JsonResult GetDataForChart()
        {
            var users = _context.Payments.Select(p => p.UserId).Distinct().Count();
            var trees = _context.Payments.Where(p => p.IsPaymentSucceded).Sum(p => p.PurchasedAmount);
            var remainingTrees = _context.Seedlings.Sum(s => s.Amount) - trees;
            return new JsonResult(new {users = users, trees = trees, remainingTrees = remainingTrees });
        }
    }
}
