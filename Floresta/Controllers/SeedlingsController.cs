using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    [Authorize(Roles = "admin")]
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

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var seedling = await _context.Seedlings.FirstOrDefaultAsync(x => x.Id == id);
                return View(seedling);
            }
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Seedling seedling)
        {
            _context.Seedlings.Update(seedling);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
