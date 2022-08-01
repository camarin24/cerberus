using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cerberus.Domain.Dtos.Auth;

namespace Cerberus.Domain.Ports.Auth;

public interface IUserPermissionService : IBaseService<UserPermissionDto>
{
    Task<List<UserPermissionDto>> GetPermissionByUserId(Guid userId);
}