using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Core.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> Table { get; }

        Task<IList<T>> GetAllAsync();

        Task InsertAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
