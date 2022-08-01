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
            var clients = await Query(query, new
            {
                ClientId = clientId,
                ApplicationId = applicationId
            });
            return clients.ToList();
        }
    }
}