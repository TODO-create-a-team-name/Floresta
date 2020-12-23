using Floresta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    [Authorize(Roles = "admin")]
    public class SeedlingsController : Controller
    {
        private FlorestaDbContext _context;

        public SeedlingsController(FlorestaDbContext context)
        {
            _context = context;
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

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var seedling = _context.Seedlings.FirstOrDefault(x => x.Id == id);
            if(seedling != null)
            {
                _context.Seedlings.Remove(seedling);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
