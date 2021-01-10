using Floresta.Interfaces;
using Floresta.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Floresta.Repositories
{
    public class MarkerRepository : IRepository<Marker>
    {
        private FlorestaDbContext _context;
        public MarkerRepository(FlorestaDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Marker> GetAll()
        => _context.Markers;
        public Marker GetById(int? id)
        => GetAll().FirstOrDefault(x => x.Id == id);

        public async Task AddAsync(Marker newMarker)
        {
            await _context.Markers.AddAsync(newMarker);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Marker marker)
        {
            _context.Markers.Update(marker);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var marker = GetById(id);

            if (marker != null)
            {
                _context.Markers.Remove(marker);
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
