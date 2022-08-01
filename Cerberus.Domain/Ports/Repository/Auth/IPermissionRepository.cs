using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cerberus.Domain.Entities;

namespace Cerberus.Domain.Ports.Repository.Auth;

public interface IPermissionRepository : IGenericRepository<Permission>
{
    Task<IEnumerable<Permission>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId);
}