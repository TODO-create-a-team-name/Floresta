﻿using Floresta.Models;
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
            //News news = _context.News.OrderByDescending(i => i.Id).SingleOrDefault();


            //var list = _context.News.ToList();
            //foreach (var i in list)
            //{
            //    string imageBase64Data = Convert.ToBase64String(i.ImageByte);
            //    string imageDataURL = string.Format("data:image/jpg;base64,{0}", imageBase64Data);
            //    i.Image = imageDataURL;
            //}
            return View(await _context.News.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Context,ImageFile")] News news)
        {
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image/news
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(news.ImageFile.FileName);
                string extension = Path.GetExtension(news.ImageFile.FileName);
                news.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Images/news", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await news.ImageFile.CopyToAsync(fileStream);
                }
                //Insert record
                _context.Add(news);
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
        public async Task<IActionResult> Edit([Bind("Id,Title,Context,Image,ImageFile")] News news)
        {
            if (ModelState.IsValid)
            {
                if(news.ImageFile!=null)
                {
                    var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images/news", news.Image);
                    if (System.IO.File.Exists(imagePath))
                        System.IO.File.Delete(imagePath);
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(news.ImageFile.FileName);
                    string extension = Path.GetExtension(news.ImageFile.FileName);
                    news.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/Images/news", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await news.ImageFile.CopyToAsync(fileStream);
                    }
                }
                //Save image to wwwroot/image/news
                
                //Insert record
                _context.Update(news);
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

            //delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "images/news", news.Image);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
            //delete the record
            _context.News.Remove(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}