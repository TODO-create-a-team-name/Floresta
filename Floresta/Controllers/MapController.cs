using Floresta.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class MapController : Controller
    {
        private FlorestaDbContext db;
        public MapController(FlorestaDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Index(Marker marker)
        //{
        //    db.Markers.Add(marker);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
    }
}
