using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class NewsController : Controller
    {
        private FlorestaDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public NewsController(FlorestaDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment; 
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.News.ToListAsync());
        }
        public async Task<IActionResult> AdminIndex()
        {
            return View(await _context.News.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        public async Task<News> SaveImageAsync(News news)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            //Save image to wwwroot/image/news
            if(news.ImageFile == null)
            {
                news.Image = "/default.png";
            }
            else
            {
                string fileName = Path.GetFileNameWithoutExtension(news.ImageFile.FileName);
                string extension = Path.GetExtension(news.ImageFile.FileName);
                news.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/news", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await news.ImageFile.CopyToAsync(fileStream);
                }
            }
            return news;
        }

        public News DeleteImage(News news)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images/news", news.Image);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            return news;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Context,ImageFile")] News news)
        {
            if (ModelState.IsValid)
            {
                await SaveImageAsync(news);
                await _context.News.AddAsync(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var news = await _context.News.FirstOrDefaultAsync(x => x.Id == id);
                return View(news);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(News model)
        {
            var news = await _context.News.FirstOrDefaultAsync(x => x.Id == model.Id);
            if(news != null)
            {
                news.Title = model.Title;
                news.Context = model.Context;
                news.Image = model.Image;
            }
            if (ModelState.IsValid)
            {
                if(news.ImageFile!=null)
                {
                    // delete old image
                    DeleteImage(news);
                    //save new image
                    await SaveImageAsync(news);
                }
                _context.News.Update(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);

            //delete image from wwwroot/images/news
            DeleteImage(news);
            //delete the record
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
