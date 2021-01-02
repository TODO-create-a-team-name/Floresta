using Floresta.Interfaces;
using Floresta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Repositories
{
    public class SeedlingRepository : IRepository<Seedling>
    {
        private FlorestaDbContext _context;
        public SeedlingRepository(FlorestaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Seedling> GetAll()
        => _context.Seedlings;

        public Seedling GetById(int? id)
        => GetAll().FirstOrDefault(x => x.Id == id);   

        public async Task AddAsync(Seedling newSeedling)
        {
            await _context.Seedlings.AddAsync(newSeedling);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Seedling seedling)
        {
            _context.Seedlings.Update(seedling);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var seedling = GetById(id);

            if(seedling != null)
            {
                _context.Seedlings.Remove(seedling);
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
