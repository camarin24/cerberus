using System;
using System.Collections.Generic;
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

        public async Task<List<UserPermissionDto>> GetPermissionByUserId(int userId)
        {
            var permissions = await GetFilter(m => m.UserId == userId);
            if (permissions == null) return new List<UserPermissionDto>();
            return _mapper.Map<List<UserPermissionDto>>(permissions);
        }

        public async Task<IEnumerable<UserPermission>> GetFilter(Expression<Func<UserPermission, bool>> filter)
        {
            return await _repository.Get(filter);
        }
    }
}