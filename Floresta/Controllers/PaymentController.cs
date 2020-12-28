using Floresta.Models;
using Floresta.Services;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class PaymentController : Controller
    {
        private FlorestaDbContext _context;
        private UserManager<User> _userManager;

        public PaymentController(FlorestaDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(PaymentViewModel m)
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
            EmailService emailService = new EmailService();

            await emailService.SendEmailAsync(user.Email, "Purchase status",
                $"Dear {user.Name} {user.UserSurname}, thank you for purchasing! You will receive an e-mail about the purchase status as soon as it's possible!");
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
            if(marker.PlantCount==0)
            {
                marker.isPlantingFinished = true;
                _context.Update(marker);
            }
            _context.Add(payment);
            _context.SaveChanges();
            

            return RedirectToAction("Index", "Map");
        }

        [Authorize(Roles = "admin")]
        public IActionResult SendEmail()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> SendEmail(string id, SendEmailViewModel model)
        {
            
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                EmailService emailService = new EmailService();

                await emailService.SendEmailAsync(user.Email, "Purchase status",
                    model.Message);
                return Ok();
            }
            return NotFound();
        }
    }
}
