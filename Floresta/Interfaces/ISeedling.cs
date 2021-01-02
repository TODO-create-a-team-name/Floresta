using Floresta.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Floresta.Interfaces
{
    public interface ISeedling
    {
        IEnumerable<Seedling> GetAllSeedlings();
        Seedling GetById(int? id);
        Task AddSeedlingAsync(Seedling newSeedling);
        Task UpdateSeedingAsync(Seedling seedling);
        Task<bool> DeleteSeedingAsync(int id);

    }
}
