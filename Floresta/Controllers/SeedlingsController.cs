using Floresta.Interfaces;
using Floresta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    [Authorize(Roles = "admin")]
    public class SeedlingsController : Controller
    {
        private IRepository<Seedling> _repo;

        public SeedlingsController(IRepository<Seedling> repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {            
            return View(_repo.GetAll());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Seedling seedling)
        {
            await _repo.AddAsync(seedling);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int? id)
        {
            if (id != null)
            {
                var seedling = _repo.GetById(id);
                return View(seedling);
            }
                return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Seedling seedling)
        {
            await _repo.UpdateAsync(seedling);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if(await _repo.DeleteAsync(id))
                return RedirectToAction("Index");
            else
            return NotFound();
        }
    }
}
