using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class MapController : Controller
    {
        private FlorestaDbContext _context;
        private SignInManager<User> _signInManager;
        public MapController(FlorestaDbContext context, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var seedlings = _context.Seedlings.Where(s => s.Amount != 0).ToList();
                var model = new PaymentViewModel();
                model.Seedlings = seedlings;
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }
        [Authorize(Roles = "admin")]
        public IActionResult Markers()
        {
            var list = _context.Markers.ToList();
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> Index(Marker marker)
        {
            _context.Markers.Add(marker);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var markers = await _context.Markers.FirstOrDefaultAsync(x => x.Id == id);
                return View(markers);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Marker marker)
        {
            _context.Markers.Update(marker);
            await _context.SaveChangesAsync();
            return RedirectToAction("Markers");
        }

        

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marker = _context.Markers.FirstOrDefault(x => x.Id == id);
            if (marker != null)
            {
                _context.Markers.Remove(marker);
                await _context.SaveChangesAsync();
                return RedirectToAction("Markers");
            }
            return NotFound();
        }

        public JsonResult GetMarkers()
        {
            var markers = _context.Markers.ToList();
            return new JsonResult(markers);
        }
        public JsonResult GetRequiredData()
        {
            var markers = _context.Markers.ToList();
            var seedlings = _context.Seedlings.ToList();
            bool IsAdmin = _signInManager.IsSignedIn(User) && User.IsInRole("admin");

            return new JsonResult(new
            {
                markers = markers,
                seedlings = seedlings,
                IsAdmin = IsAdmin
            });
        }
    }
}
