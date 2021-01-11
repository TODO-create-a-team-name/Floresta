using AutoMapper;
using Floresta.Interfaces;
using Floresta.Models;
using Floresta.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Floresta.Controllers
{
    public class QuestionTopicController : Controller
    {
        private IRepository<QuestionTopic> _repo;
        private readonly IMapper _mapper;
        public QuestionTopicController(IRepository<QuestionTopic> repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var topics = _mapper.Map<IEnumerable<QuestionTopicViewModel>>(_repo.GetAll());
            return View(topics);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionTopicViewModel model)
        {
            if(ModelState.IsValid)
            {
                var topic = _mapper.Map<QuestionTopic>(model);
                await _repo.AddAsync(topic);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            var topic = _repo.GetById(id);
            var model = _mapper.Map<QuestionTopicViewModel>(topic);
            if (topic == null)
                return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionTopicViewModel model)
        {
            await _repo.UpdateAsync(_mapper.Map<QuestionTopic>(model));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _repo.DeleteAsync(id))
                return RedirectToAction("Index");
            else
                return NotFound();
        }
    }
}
