using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Ports.Auth;
using Cerberus.Domain.Ports.Repository;

namespace Cerberus.Domain.Services.Auth;

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