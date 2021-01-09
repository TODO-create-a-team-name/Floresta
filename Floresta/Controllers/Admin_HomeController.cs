using Floresta.Models;
using Floresta.Services;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Linq;
using System.Threading.Tasks;


namespace Floresta.Controllers
{
    [Authorize(Roles = "admin, moderator")]
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

        public IActionResult GetQuestions()
        {
            var questions = _context.Questions.Include(c => c.User);
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

            await emailService.SendEmailAsync(user.Email, "Відповідь на питання",
                $"Ваше питання було: \"{question.QuestionText}\"\n\nОфіційна відповідь на ваше питання:\n\n{model.Message}");
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
                .Include(s => s.Seedling);

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

                await emailService.SendEmailAsync(user.Email, "Вітання!!!",
                    $"Дорога(-ий) {user.Name} {user.UserSurname}, ваша оплата була успішно підтверджена!" +
                    $"\nВаше бажання врятувати світ є більшим, ніж наша вдячність вам!\nСлідкуйте за нашими оновленнями, щоб бути в курсі всього!");
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
                await emailService.SendEmailAsync(user.Email, "Статус Оплати",
                    $"{user.Name} {user.UserSurname}, на жаль, ваша оплата не була успішною. Зв'яжіться з підтримкою Floresta для отримання більш детальної інформації.");
                
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

                await emailService.SendEmailAsync(user.Email, "Статус участі в команді",
                    $"{user.Name} {user.UserSurname}, ви тепер офіційно учасник команди Floresta Team!<br /> " +
                    $"Давайте зробимо світ чистішим киснем наших дерев!");
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
                user.IsTeamParticipant = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(user.Email, "Статус участі в команді",
                    $"{user.Name} {user.UserSurname}, на жаль, ви не стали учасником Floresta Team, " +
                    $"проте ви однаково зможете зробити світ кращим, посадивши дерево!");
                return RedirectToAction("GetTeamParticipants");
            }
            else
                return NotFound();
        }

        public JsonResult GetSeedlingsRates()
        {
            var statistics = _context.Payments.Include(s => s.Seedling)
                .GroupBy(p => p.Seedling.Name)
                .Select(p => new { seedling = p.Key, sum = p.Sum(p => p.PurchasedAmount) })
                .AsEnumerable()
                .ToDictionary(d => d.seedling, d => d.sum);
         
            return new JsonResult(statistics);
        }
    }
}
