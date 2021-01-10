using System.Collections.Generic;
using System.Threading.Tasks;

namespace Floresta.Interfaces
{
    public interface IRepository<T> 
    {
        IEnumerable<T> GetAll();
        T GetById(int? id);
        Task AddAsync(T newItem);
        Task UpdateAsync(T item);
        Task<bool> DeleteAsync(int id);
    }
}
