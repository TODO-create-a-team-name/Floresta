﻿using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class PaymentController : Controller
    {
        private FlorestaDbContext _context;
        private UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public PaymentController(FlorestaDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IActionResult> Index(PaymentViewModel m)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var marker = await _context.Markers.FirstOrDefaultAsync(x => x.Id == m.MarkerId);
                var seedling = await _context.Seedlings.FirstOrDefaultAsync(x => x.Id == m.SeedlingId);
                var model = new ConfirmPaymentViewModel()
                {
                    MarkerTitle = marker.Title,
                    PurchasedAmount = m.PlantCount,
                    Price = m.PlantCount * seedling.Price,
                    Seedling = seedling.Name,
                    Name = user.Name,
                    Surname = user.UserSurname,
                    Email = user.Email
                };

                return View(model);
            }
            else
            {
                RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(User);
            //var marker = await _context.Markers.FirstOrDefaultAsync(x => x.Id == paymentViewModel.MarkerId);
            //var seedling = await _context.Seedlings.FirstOrDefaultAsync(x => x.Id == paymentViewModel.SeedlingId);
            //var payment = new Payment
            //{
            //    User = user,
            //    Marker = marker,
            //    Seedling = seedling,
            //    PurchasedAmount = paymentViewModel.PlantCount,
            //    Price = paymentViewModel.PlantCount * seedling.Price
            //};
            //_context.Add(payment);
            //_context.SaveChanges();
            return RedirectToAction("Index", "Map");
        }
    }
}