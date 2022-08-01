using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Ports.Auth;
using Cerberus.Domain.Ports.Repository;
using Mapster;

namespace Cerberus.Domain.Services.Auth;

[DomainService]
public class UserPermissionService : BaseService<UserPermission, UserPermissionDto>, IUserPermissionService
{
    private readonly IGenericRepository<UserPermission> _repository;

    public UserPermissionService(IGenericRepository<UserPermission> repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<List<UserPermissionDto>> GetPermissionByUserId(Guid userId)
    {
        return (await _repository.Where(new {UserId = userId})).Adapt<List<UserPermissionDto>>();
    }
}