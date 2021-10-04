using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Intecgra.Cerberus.Domain.Ports.Data
{
    public interface IGenericRepository<TE> where TE : class
    {
        Task<IEnumerable<TE>> Get(Expression<Func<TE, bool>> filter = null,
            Func<IQueryable<TE>, IOrderedQueryable<TE>> orderBy = null, string includeProperties = "", bool isTracking = false);
        Task<TE> GetById(object id);
        Task<TE> Save(TE entity);
        Task SaveRange(IEnumerable<TE> entity);
        Task Update(TE entity);
        Task UpdateRange(IEnumerable<TE> entity);
        Task Delete(TE entity);
        Task DeleteRange(IEnumerable<TE> entity);
    }
}