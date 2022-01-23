using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;

namespace Cerberus.Domain.Ports.Auth
{
    public interface IClientService : IBaseService<ClientDto>
    {
        /// <summary>
        /// Verify if the current client has access to the specified application
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<ClientDto> GetClientByAppIdAndId(Guid appId, Guid clientId);
    }
}