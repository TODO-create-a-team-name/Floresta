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
        private UserManager<User> _userManager;
        private FlorestaDbContext _context;

        public HomeController(UserManager<User> userManager,
            FlorestaDbContext context)
        {

            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new HomeViewModel();

            return View(model);
               
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AskQuestion(HomeViewModel model)
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
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
                return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> JoinTeam(HomeViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if(user != null)
            {
                if (model.PhoneNumber != null)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.IsClaimingForTeamParticipating = true;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public JsonResult GetDataForChart()
        {
            var users = _context.Payments.Where(p => p.IsPaymentSucceded).Select(p => p.UserId).Distinct().Count();
            var trees = _context.Payments.Where(p => p.IsPaymentSucceded).Sum(p => p.PurchasedAmount);
            var remainingTrees = _context.Seedlings.Sum(s => s.Amount);
            return new JsonResult(new {users, trees, remainingTrees });
        }
    }
}
