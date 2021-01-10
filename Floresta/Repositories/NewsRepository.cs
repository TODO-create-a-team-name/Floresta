using Floresta.Interfaces;
using Floresta.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Repositories
{
    public class NewsRepository : IRepository<News>
    {
        private FlorestaDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public NewsRepository(FlorestaDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        public IEnumerable<News> GetAll()
            => _context.News;

        public News GetById(int? id)
            => GetAll().FirstOrDefault(x => x.Id == id);

        public async Task AddAsync(News news)
        {
            news.DatePublished = DateTime.Now;
            await SaveImageAsync(news);
            await _context.News.AddAsync(news);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(News news)
        {
            if(news.ImageFile != null)
            {
                DeleteImage(news);
                await SaveImageAsync(news);
            }
            _context.News.Update(news);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var news = GetById(id);
            if (news != null)
            {
                DeleteImage(news);
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }

        private void DeleteImage(News news)
        {
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath,
                "images/news",
                news.Image);

            if (File.Exists(imagePath))
                File.Delete(imagePath);
        }

        private async Task<News> SaveImageAsync(News news)
        {
            string wwwRootPath = _hostEnvironment.WebRootPath;
            //Save image to wwwroot/image/news
            if (news.ImageFile == null)
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

    }
}
