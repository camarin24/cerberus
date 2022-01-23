using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Exceptions;
using Cerberus.Domain.Ports.Auth;
using Cerberus.Domain.Ports.Repository.Auth;
using Cerberus.Domain.Utilities;

namespace Cerberus.Domain.Services.Auth
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