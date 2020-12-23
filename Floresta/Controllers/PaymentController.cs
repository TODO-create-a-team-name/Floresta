using Floresta.Models;
using Floresta.Services;
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
        [HttpGet]
        public async Task<IActionResult> Index(PaymentViewModel m)
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var marker = await _context.Markers.FirstOrDefaultAsync(x => x.Id == m.MarkerId);
                var seedling = await _context.Seedlings.FirstOrDefaultAsync(x => x.Id == m.SeedlingId);
                var model = new ConfirmPaymentViewModel()
                {
                    MarkerId = m.MarkerId,
                    SeedlingId = m.SeedlingId,
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
        public async Task<IActionResult> Index(ConfirmPaymentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            var marker = await _context.Markers.FirstOrDefaultAsync(x => x.Id == model.MarkerId);
            var seedling = await _context.Seedlings.FirstOrDefaultAsync(x => x.Id == model.SeedlingId);
            var payment = new Payment
            {
                User = user,
                Marker = marker,
                Seedling = seedling,
                PurchasedAmount = model.PurchasedAmount,
                Price = model.Price
            };
            if (seedling.Amount > 0)
            {
                seedling.Amount -= model.PurchasedAmount;
                _context.Update(seedling);
            }
            if (marker.PlantCount > 0)
            {
                marker.PlantCount -= model.PurchasedAmount;
                _context.Update(marker);
            }
            _context.Add(payment);
            _context.SaveChanges();
            EmailService emailService = new EmailService();

            await emailService.SendEmailAsync(user.Email, "Purchase status",
                $"Dear {user.Name} {user.UserSurname}, thank you for purchasing! You will receive an e-mail about the purchase status as soon as it's possible!");

            return RedirectToAction("Index", "Map");
        }
    }
}
