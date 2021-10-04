using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;

namespace Intecgra.Cerberus.Domain.Ports.Auth
{
    public interface IUserService : IBaseService<UserDto>
    {
        Task<AuthorizationDto> Login(LoginDto request);
        Task<IEnumerable<UserDto>> GetFilter(Expression<Func<User, bool>> filter);
    }
}