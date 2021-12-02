using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Repository;

namespace Intecgra.Cerberus.Domain.Services.Auth;

[DomainService]
public class UserPermissionService : BaseService<UserPermission, UserPermissionDto>, IUserPermissionService
{
    private readonly IGenericRepository<UserPermission> _repository;
    private readonly IMapper _mapper;

    public UserPermissionService(IGenericRepository<UserPermission> repository, IMapper mapper) : base(repository,
        mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<UserPermissionDto>> GetPermissionByUserId(Guid userId)
    {
        var permissions =
            await _repository.Get(where: new Dictionary<string, dynamic>() {{"user_id", userId}});
        if (permissions == null) return new List<UserPermissionDto>();
        return _mapper.Map<List<UserPermissionDto>>(permissions);
    }
}