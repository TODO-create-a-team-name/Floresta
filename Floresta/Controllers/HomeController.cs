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

        public HomeController(SignInManager<User> signInManager,
            UserManager<User> userManager,
            FlorestaDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new QuestionViewModel();

            return View(model);
               
        }
        public IActionResult GetQuestionPart()
        {
            return PartialView("_GetQuestionPart");
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

        public JsonResult GetDataForChart()
        {
            var users = _context.Payments.Select(p => p.UserId).Distinct().Count();
            var trees = _context.Payments.Where(p => p.IsPaymentSucceded).Sum(p => p.PurchasedAmount);
            var remainingTrees = _context.Seedlings.Sum(s => s.Amount) - trees;
            return new JsonResult(new {users = users, trees = trees, remainingTrees = remainingTrees });
        }
    }
}
