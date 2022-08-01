using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cerberus.Domain.Ports.Repository;

public interface IGenericRepository<TE> where TE : class
{
    Task<IEnumerable<TE>> GetAll();
    Task<IEnumerable<TE>> Where(object whereConditions, object? inCondition = null);
    Task<TE> GetById(object id);
    Task<TK> Create<TK>(TE entity);
    Task Create(IEnumerable<TE> entities);
    Task<bool> Update(TE entity);
    Task<bool> Update(IEnumerable<TE> entities);
    Task<bool> Delete(int entityId);
    Task<IEnumerable<TE>> Query(string query, object parameters);
}