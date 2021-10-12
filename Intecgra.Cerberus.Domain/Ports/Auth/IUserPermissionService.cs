using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;

namespace Intecgra.Cerberus.Domain.Ports.Auth
{
    public interface IUserPermissionService : IBaseService<UserPermissionDto>
    {
        Task<List<UserPermissionDto>> GetPermissionByUserId(Guid userId);
        Task<IEnumerable<UserPermission>> GetFilter(Expression<Func<UserPermission, bool>> filter);
    }
}