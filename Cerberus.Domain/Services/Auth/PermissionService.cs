using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Ports.Auth;
using Cerberus.Domain.Ports.Repository.Auth;
using Mapster;

namespace Cerberus.Domain.Services.Auth;

[DomainService]
public class PermissionService : BaseService<Permission, PermissionDto>, IPermissionService
{
    private readonly IPermissionRepository _repository;
    private readonly IUserPermissionService _userPermissionService;

    public PermissionService(IPermissionRepository repository,
        IUserPermissionService userPermissionService) : base(repository)
    {
        _repository = repository;
        _userPermissionService = userPermissionService;
    }

    public async Task<List<PermissionDto>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId)
    {
        var permissions = await _repository.GetPermissionsByApplicationAndUser(appId, userId);
        return permissions.Adapt<List<PermissionDto>>();
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsByName(Guid appId, IEnumerable<string> names)
    {
        var permissions = await _repository.Where(new {ApplicationId = appId}, new {Name = names.ToArray()});
        return permissions.Adapt<IEnumerable<PermissionDto>>();
    }
}