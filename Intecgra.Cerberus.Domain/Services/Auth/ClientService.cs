using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Exceptions;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Repository.Auth;
using Intecgra.Cerberus.Domain.Utilities;

namespace Intecgra.Cerberus.Domain.Services.Auth
{
    [DomainService]
    public class ClientService : BaseService<Client, ClientDto>, IClientService
    {
        private readonly IClientRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMessagesManager _messagesManager;

        public ClientService(IClientRepository repository, IMapper mapper, IMessagesManager messagesManager) :
            base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _messagesManager = messagesManager;
        }

        public async Task<ClientDto> GetClientByAppIdAndId(Guid appId, Guid clientId)
        {
            var clients = await _repository.GetByIdAndApplicationId(clientId, appId);
            if (!clients.Any()) throw new DomainException(_messagesManager.GetMessage("UnauthorizedApplicationClient"));
            return _mapper.Map<ClientDto>(clients.First());
        }
    }
}