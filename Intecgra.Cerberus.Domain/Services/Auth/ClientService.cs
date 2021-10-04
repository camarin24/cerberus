using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Exceptions;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Data;
using Intecgra.Cerberus.Domain.Utilities;

namespace Intecgra.Cerberus.Domain.Services.Auth
{
    [DomainService]
    public class ClientService : BaseService<Client, ClientDto>, IClientService
    {
        private readonly IGenericRepository<Client> _repository;
        private readonly IMapper _mapper;
        private readonly IMessagesManager _messagesManager;

        public ClientService(IGenericRepository<Client> repository, IMapper mapper, IMessagesManager messagesManager) :
            base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _messagesManager = messagesManager;
        }

        public async Task<ClientDto> GetClientByAppIdAndId(Guid appId, Guid clientId)
        {
            var clients = (await GetFilter(m =>
                m.ClientId == clientId && m.ClientApplications.Any(cp => cp.ApplicationId == appId))).ToList();
            if (!clients.Any()) throw new DomainException(_messagesManager.GetMessage("UnauthorizedApplicationClient"));
            return _mapper.Map<ClientDto>(clients.First());
        }

        public async Task<IEnumerable<Client>> GetFilter(Expression<Func<Client, bool>> filter)
        {
            return await _repository.Get(filter);
        }
    }
}