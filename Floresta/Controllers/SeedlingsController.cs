using Floresta.Interfaces;
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
        private ISeedling _service;

        public SeedlingsController(ISeedling service)
        {
            _service = service;
        }
        public IActionResult Index()
        {            
            return View(_service.GetAllSeedlings());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Seedling seedling)
        {
            await _service.AddSeedlingAsync(seedling);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var seedling = _service.GetById(id);
                return View(seedling);
            }
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Seedling seedling)
        {
            await _service.UpdateSeedingAsync(seedling);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _service.DeleteSeedingAsync(id))
                return RedirectToAction("Index");
            else
            return NotFound();
        }
    }
}
