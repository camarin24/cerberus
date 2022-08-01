using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Repository.Data;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Ports.Repository.Auth;
using Microsoft.Extensions.Configuration;

namespace Cerberus.Repository.Adapters.Auth
{
    public class PermissionRepository : GenericRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId)
        {
            var query = @"select * from auth.permission p 
                            join auth.user_permission up on p.permission_id = up.permission_id
                            where p.application_id = @ApplicationId and up.user_id = @UserId";
            var queryParams = new
            {
                ApplicationId = appId,
                UserId = userId,
            };
            return await Query(query, queryParams);
        }
    }
}