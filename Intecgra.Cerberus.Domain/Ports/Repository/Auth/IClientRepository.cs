using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Entities;

namespace Intecgra.Cerberus.Domain.Ports.Repository.Auth
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<IList<Client>> GetByIdAndApplicationId(Guid clientId, Guid applicationId);
    }
}