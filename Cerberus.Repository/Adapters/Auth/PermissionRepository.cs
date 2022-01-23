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

        public async Task<List<Permission>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId)
        {
            var query = @"select * from auth.permission p 
                            join auth.user_permission up on p.permission_id = up.permission_id
                            where p.application_id = @applicationId and up.user_id = @userId";
            var queryParams = new Dictionary<string, dynamic>
            {
                {"applicationId", appId},
                {"userId", userId},
            };
            return (await ExecuteQuery(query, queryParams)).ToList();
        }
    }
}