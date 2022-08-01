using System;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Exceptions;
using Cerberus.Domain.Ports.Auth;
using Cerberus.Domain.Ports.Repository.Auth;
using Cerberus.Domain.Utilities;
using Mapster;

namespace Cerberus.Domain.Services.Auth;

[DomainService]
public class ClientService : BaseService<Client, ClientDto>, IClientService
{
    private readonly IMessagesManager _messagesManager;
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository, IMessagesManager messagesManager) :
        base(repository)
    {
        _repository = repository;
        _messagesManager = messagesManager;
    }

    public async Task<ClientDto> GetClientByAppIdAndId(Guid appId, Guid clientId)
    {
        var clients = await _repository.GetByIdAndApplicationId(clientId, appId);
        if (!clients.Any()) throw new DomainException(_messagesManager.GetMessage("UnauthorizedApplicationClient"));
        return clients.First().Adapt<ClientDto>();
    }
}