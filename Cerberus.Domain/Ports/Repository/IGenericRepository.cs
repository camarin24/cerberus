using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cerberus.Domain.Ports.Repository;

public interface IGenericRepository<TE> where TE : class
{
    Task<IEnumerable<TE>> GetAll();
    Task<IEnumerable<TE>> Get(string query = null, Dictionary<string, dynamic> where = null);

    Task<IEnumerable<TE>> GetIn(string query = null, Dictionary<string, dynamic> @in = null,
        dynamic data = null, bool inferPk = false);
    Task<IEnumerable<TE>> ExecuteQuery(string query, Dictionary<string, dynamic> queryParams);
    Task<TE> GetById(object id);
    Task<TP> Save<TP>(TE entity);
    Task<IEnumerable<TP>> SaveRange<TP>(IEnumerable<TE> entity);
    Task Update(TE entity);
    Task UpdateRange(IEnumerable<TE> entity);
    Task Delete(TE entity);
    Task DeleteRange(IEnumerable<TE> entity);
}