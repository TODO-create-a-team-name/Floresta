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
        private FlorestaDbContext _context;
        public MapController(FlorestaDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Marker marker)
        {
            _context.Markers.Add(marker);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public JsonResult GetMarkers()
        {
            var markers = _context.Markers.ToList();
            return new JsonResult(markers);
        }
    }
}
