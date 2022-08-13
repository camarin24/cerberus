using System;
using System.Threading.Tasks;
using Cerberus.Contracts.Auth;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Exceptions;

namespace Cerberus.Domain.Ports.Auth;

public interface IUserService : IBaseService<UserDto>
{
    /// <summary>
    ///     Login the user through the specific client database
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    /// <exception cref="Exception"></exception>
    Task<AuthorizationContract> Login(LoginDto request);

    Task<MeResponseContract> Me(MeRequestContract request);
    Task<AuthorizationContract> RefreshToken(MeRequestContract request);
    Task<UserDto> CreateUserWithPermissions(CreateUserWithPermissionsContract request);
    Task<UserDto> CreateUser(UserDto dto);
}