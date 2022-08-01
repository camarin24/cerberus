using System;
using System.Threading.Tasks;
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
    Task<AuthorizationDto> Login(LoginDto request);

    Task<MeResponseDto> Me(MeRequestDto request);
    Task<AuthorizationDto> RefreshToken(MeRequestDto request);
    Task<UserDto> CreateUserWithPermissions(CreateUserWithPermissionsDto request);
    Task<UserDto> CreateUser(UserDto dto);
}