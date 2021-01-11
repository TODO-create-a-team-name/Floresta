using Floresta.Interfaces;
using Floresta.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Repositories
{
    public class QuestionTopicRepository : IRepository<QuestionTopic>
    {
        private FlorestaDbContext _context;
        public QuestionTopicRepository(FlorestaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<QuestionTopic> GetAll()
        => _context.QuestionTopics;

        public QuestionTopic GetById(int? id)
        => GetAll().FirstOrDefault(x => x.Id == id);

        public async Task AddAsync(QuestionTopic item)
        {
            await _context.QuestionTopics.AddAsync(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(QuestionTopic item)
        {
            _context.QuestionTopics.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var topic = GetById(id);

            if (topic != null)
            {
                _context.QuestionTopics.Remove(topic);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
