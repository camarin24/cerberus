using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Repository.Auth;

namespace Intecgra.Cerberus.Domain.Services.Auth;

[DomainService]
public class PermissionService : BaseService<Permission, PermissionDto>, IPermissionService
{
    private readonly IPermissionRepository _repository;
    private readonly IMapper _mapper;
    private readonly IUserPermissionService _userPermissionService;

    public PermissionService(IPermissionRepository repository, IMapper mapper,
        IUserPermissionService userPermissionService) : base(repository, mapper)
    {
        _repository = repository;
        _mapper = mapper;
        _userPermissionService = userPermissionService;
    }

    public async Task<List<PermissionDto>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId)
    {
        var permissions = await _repository.GetPermissionsByApplicationAndUser(appId, userId);
        return _mapper.Map<List<PermissionDto>>(permissions);
    }

    public async Task<IEnumerable<PermissionDto>> GetPermissionsByName(Guid appId, IEnumerable<string> names)
    {
        var permissions = await _repository.GetIn(@in: new Dictionary<string, dynamic>
        {
            {"name", names.ToArray()},
            {"application_id", new[] {appId}}
        });
        return _mapper.Map<List<PermissionDto>>(permissions);
    }
}