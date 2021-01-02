using Floresta.Interfaces;
using Floresta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Services
{
    public class SeedlingService : ISeedling
    {
        private FlorestaDbContext _context;
        public SeedlingService(FlorestaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Seedling> GetAllSeedlings()
        => _context.Seedlings;

        public Seedling GetById(int? id)
        => GetAllSeedlings().FirstOrDefault(x => x.Id == id);   

        public async Task AddSeedlingAsync(Seedling newSeedling)
        {
            await _context.Seedlings.AddAsync(newSeedling);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteSeedingAsync(int id)
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

        public async Task UpdateSeedingAsync(Seedling seedling)
        {
            _context.Seedlings.Update(seedling);
            await _context.SaveChangesAsync();
        }
    }
}
