using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Intecgra.Cerberus.Domain.Ports.Data
{
    public interface IDapperGenericRepository<TE> where TE : class
    {
        Task<IEnumerable<TE>> GetAll();
        Task<IEnumerable<TE>> Get(string query = null, Dictionary<string, dynamic> where = null);
        Task<IEnumerable<TE>> GetIn(string query = null, object @in = null);
        Task<TE> GetById(object id);
        Task<TP> Save<TP>(TE entity);
        Task SaveRange(IEnumerable<TE> entity);
        Task Update(TE entity);
        Task UpdateRange(IEnumerable<TE> entity);
        Task Delete(TE entity);
        Task DeleteRange(IEnumerable<TE> entity);
    }
}