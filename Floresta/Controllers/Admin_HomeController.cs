using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;


namespace Floresta.Controllers
{
    public class Admin_HomeController : Controller
    {
        SignInManager<User> _signInManager;
        UserManager<User> _userManager;

        public Admin_HomeController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var model = new ShowUserViewModel
                    {
                        Name = user.Name,
                        Surname = user.UserSurname,
                        Email = user.Email
                    };

                    return View(model);
                }
            }
            return View();
        }
    }
}
