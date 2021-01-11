using Floresta.Interfaces;
using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class MapController : Controller
    {
        private IRepository<Marker> _repo;
        private FlorestaDbContext _context;
        private SignInManager<User> _signInManager;

        public MapController(IRepository<Marker> repo,
            FlorestaDbContext context,
            SignInManager<User> signInManager)
        {
            _repo = repo;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var seedlings = _context.Seedlings.Where(s => s.Amount > 0).ToList();
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
            var markers = _repo.GetAll();
            return View(markers);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Index(Marker marker)
        {
            await _repo.AddAsync(marker);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var marker = _repo.GetById(id);
                return View(marker);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Marker marker)
        {
            await _repo.UpdateAsync(marker);
            return RedirectToAction("Markers");
        }

        

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        { 
            if (await _repo.DeleteAsync(id))
            {
                return RedirectToAction("Markers");
            }
            else
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
            var seedlings = _context.Seedlings.Select(s => new { s.Id, s.Amount});
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
