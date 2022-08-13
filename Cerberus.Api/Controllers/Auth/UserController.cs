using System.Threading.Tasks;
using Cerberus.Contracts.Auth;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Ports.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cerberus.Api.Controllers.Auth;

[Route("api/user")]
[ApiController]
[AllowAnonymous]
public class UserController : BaseController<UserDto>
{
    private readonly IUserService _service;

    public UserController(IUserService service) : base(service)
    {
        _service = service;
    }

    [HttpPost("create")]
    public async Task<UserDto> Create(CreateUserContract request)
    {
        var user = new UserDto
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
            ClientId = request.ClientId
        };
        return await _service.CreateUser(user);
    }


    [HttpPost("create-with-permissions")]
    public async Task<UserDto> CreateWithPermissions(CreateUserWithPermissionsContract request)
    {
        return await _service.CreateUserWithPermissions(request);
    }

    [HttpPost("login")]
    public async Task<AuthorizationContract> Login(LoginDto request)
    {
        return await _service.Login(request);
    }

    [HttpPost("refresh-token")]
    public async Task<AuthorizationContract> RefreshToken(MeRequestContract request)
    {
        return await _service.RefreshToken(request);
    }

    [HttpPost("me")]
    public async Task<MeResponseContract> Me(MeRequestContract request)
    {
        return await _service.Me(request);
    }
}