using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;

namespace Cerberus.Domain.Ports.Auth
{
    public interface IUserPermissionService : IBaseService<UserPermissionDto>
    {
        Task<List<UserPermissionDto>> GetPermissionByUserId(Guid userId);
    }
}