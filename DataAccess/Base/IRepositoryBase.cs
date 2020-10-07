using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IRepositoryBase<T>
    {
        Task AddAsync(T t);
        Task<T> GetByIdAsync(int? id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(T t);
        Task DeleteAsync(T t);

        Task<bool> Exists(int? id);
    }
}
