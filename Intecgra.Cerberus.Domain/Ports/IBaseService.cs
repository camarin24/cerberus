using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Intecgra.Cerberus.Domain.Ports
{
    public interface IBaseService<TDto>
        where TDto : class
    {
        Task<IEnumerable<TDto>> Get(Dictionary<string, dynamic> where = null);

        Task<TDto> GetById(object id);
        Task<TDto> Save<TP>(TDto dt);
        Task SaveRange(IEnumerable<TDto> dto);
        Task Update(TDto entity);
        Task UpdateRange(IEnumerable<TDto> dto);
        Task Delete(TDto entity);
        Task DeleteRange(IEnumerable<TDto> dto);
        Task<TDto> Create<TP>(TDto dto);
    }
}