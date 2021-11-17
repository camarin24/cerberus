using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Entities;

namespace Intecgra.Cerberus.Domain.Ports.Repository.Auth
{
    public interface IPermissionRepository : IGenericRepository<Permission>
    {
        Task<List<Permission>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId);
    }
}