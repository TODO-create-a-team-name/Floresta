using Floresta.Interfaces;
using Floresta.Models;
using Floresta.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class NewsController : Controller
    {
        private IRepository<News> _repo;
        public NewsController(IRepository<News> repo)
        {
            _repo = repo;
        }
        public IActionResult Index()
        {
            return View(_repo.GetAll());
        }
        public IActionResult AdminIndex()
        {
            return View(_repo.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,DatePublished,ImageFile")] News news)
        {
            if (ModelState.IsValid)
            {
                await _repo.AddAsync(news);
                return RedirectToAction("Index");
            }
            return View(news);
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var news = _repo.GetById(id);
            if (news == null)
                return NotFound();
            return View(news);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(News model)
        {
            var news = _repo.GetById(model.Id);
            if (news != null)
            {
                news.Title = model.Title;
                news.Content = model.Content;
                news.Image = model.Image;
                news.ImageFile = model.ImageFile;
            }
            if (ModelState.IsValid)
            {
                await _repo.UpdateAsync(news);
                return RedirectToAction("Index");
            }
            return View(news);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _repo.DeleteAsync(id))
            {
                return RedirectToAction("Index");
            }
            else
                return NotFound();
        }

        public IActionResult ReadNews(int id)
        {
            var news = _repo.GetById(id);
            return View(news);
        }
    }
}