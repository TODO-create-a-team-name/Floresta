using Floresta.Models;
using Floresta.Services;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class QuestionsController : Controller
    {
        private UserManager<User> _userManager;
        private FlorestaDbContext _context;

        public QuestionsController(
            UserManager<User> userManager,
            FlorestaDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetQuestions()
        {
            var questions = await _context.Questions.Include(c => c.User).ToListAsync();
            return View(questions);
        }

        [Authorize(Roles = "admin")]
        public IActionResult AnswerQuestion()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AnswerQuestion(int id, SendEmailViewModel model)
        {
            Question question = _context.Questions.FirstOrDefault(x => id == x.Id);
            var user = _context.Users.FirstOrDefault(x => question.UserId == x.Id);
            EmailService emailService = new EmailService();

            await emailService.SendEmailAsync(user.Email, "The answer for your question",
                $"Your question was \"{question.QuestionText}\"\n\nThe official answer for your question:\n\n{model.Message}");
            question.IsAnswered = true;
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return RedirectToAction("GetQuestions");
        }
    }
}
