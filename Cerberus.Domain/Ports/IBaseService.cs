using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cerberus.Domain.Ports
{
    public interface IBaseService<TDto>
        where TDto : class
    {
        Task<IEnumerable<TDto>> GetAll();
        Task<TDto> GetById(object id);
        Task<TK> Create<TK>(TDto dto);
        Task Create(IEnumerable<TDto> dtos);
        Task<bool> Update(TDto dto);
        Task<bool> Delete(int id);
    }
}