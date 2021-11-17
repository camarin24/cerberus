using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Ports.Repository.Auth;
using Intecgra.Cerberus.Repository.Data;
using Microsoft.Extensions.Configuration;

namespace Intecgra.Cerberus.Repository.Adapters.Auth
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<Client>> GetByIdAndApplicationId(Guid clientId, Guid applicationId)
        {
            var query = @"select c.* from auth.client as c 
                            join auth.client_application ca on c.client_id = ca.client_id 
                            where c.client_id = @ClientId and ca.application_id = @ApplicationId";
            var queryParams = new Dictionary<string, dynamic>
            {
                {"ClientId", clientId},
                {"ApplicationId", applicationId},
            };
            var clients = await ExecuteQuery(query, queryParams);
            return clients.ToList();
        }
    }
}