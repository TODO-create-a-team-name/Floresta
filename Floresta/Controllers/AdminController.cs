using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Floresta.Controllers
{
    public class AdminController : Controller
    {
        SignInManager<User> _signInManager;
        UserManager<User> _userManager;

        public AdminController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                var model = new ShowUserViewModel
                {
                    Name = user.UserName,
                    Surname = user.UserSurname,
                    Email = user.Email
                };
                if (user != null)
                    return View(model);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AskQuestion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AskQuestion(QuestionViewModel model)
        {
            return View(model);
        }
      
    }
}
