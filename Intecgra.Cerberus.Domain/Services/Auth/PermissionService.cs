using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Data;

namespace Intecgra.Cerberus.Domain.Services.Auth
{
    [DomainService]
    public class PermissionService : BaseService<Permission, PermissionDto>, IPermissionService
    {
        private readonly IGenericRepository<Permission> _repository;
        private readonly IMapper _mapper;
        private readonly IUserPermissionService _userPermissionService;

        public PermissionService(IGenericRepository<Permission> repository, IMapper mapper,
            IUserPermissionService userPermissionService) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _userPermissionService = userPermissionService;
        }

        public async Task<List<PermissionDto>> GetPermissionsByApplicationAndUser(Guid appId, Guid userId)
        {
            var permissions = await _repository.Get(m => m.ApplicationId == appId);
            if (permissions == null) return new List<PermissionDto>();
            var userPermissions = await _userPermissionService.GetPermissionByUserId(userId);
            permissions =
                permissions.Where(m => userPermissions.Select(up => up.PermissionId).Contains(m.PermissionId));
            return _mapper.Map<List<PermissionDto>>(permissions);
        }
    }
}