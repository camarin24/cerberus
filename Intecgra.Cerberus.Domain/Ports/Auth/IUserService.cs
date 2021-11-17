using System;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Exceptions;

namespace Intecgra.Cerberus.Domain.Ports.Auth
{
    public interface IUserService : IBaseService<UserDto>
    {
        /// <summary>
        /// Login the user through the specific client database 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="DomainException"></exception>
        /// <exception cref="Exception"></exception>
        Task<AuthorizationDto> Login(LoginDto request);

        Task<MeResponseDto> Me(MeRequestDto request);

        Task<AuthorizationDto> RefreshToken(MeRequestDto request);
    }
}