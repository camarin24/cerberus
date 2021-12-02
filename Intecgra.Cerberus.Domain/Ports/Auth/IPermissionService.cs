using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Dtos.Auth;

namespace Intecgra.Cerberus.Domain.Ports.Auth;

public interface IPermissionService : IBaseService<PermissionDto>
{
    /// <summary>
    /// Get the permissions assigned to de user
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<List<PermissionDto>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId);

    /// <summary>
    /// Get a list of permissions
    /// </summary>
    /// <param name="appId"></param>
    /// <param name="names"></param>
    /// <returns></returns>
    Task<IEnumerable<PermissionDto>> GetPermissionsByName(Guid appId, IEnumerable<string> names);
}