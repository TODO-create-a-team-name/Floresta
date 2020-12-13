using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class SeedlingsController : Controller
    {
        private FlorestaDbContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public SeedlingsController(FlorestaDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            var list = _context.Seedlings.ToList();
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Seedling seedling)
        {
            _context.Seedlings.Add(seedling);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Pay(int id)
        {
            PaymentViewModel model;
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var seedling = _context.Seedlings.FirstOrDefault(x => x.Id == id);
                model = new PaymentViewModel
                {
                    Email = user.Email,
                    Seedling = seedling
                };
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Pay(string success)
        {
            success = "Payment succeded";
            return Content(success);
        }
    }
}
