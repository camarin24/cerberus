using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Intecgra.Cerberus.Api.Controllers.Auth;

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
    public async Task<UserDto> Create(CreateUserDto request)
    {
        var user = new UserDto
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
            ClientId = request.ClientId
        };
        return await _service.Create<Guid>(user);
    }

    
    [HttpPost("create-with-permissions")]
    public async Task<UserDto> CreateWithPermissions(CreateUserWithPermissionsDto request)
    {
        return await _service.CreateUserWithPermissions(request);
    }
    
    [HttpGet]
    public async Task<IEnumerable<UserDto>> Get()
    {
        return await _service.Get();
    }
    
    [HttpPost("login")]
    public async Task<AuthorizationDto> Login(LoginDto request)
    {
        return await _service.Login(request);
    }

    [HttpPost("refresh-token")]
    public async Task<AuthorizationDto> RefreshToken(MeRequestDto request)
    {
        return await _service.RefreshToken(request);
    }

    [HttpPost("me")]
    public async Task<MeResponseDto> Me(MeRequestDto request)
    {
        return await _service.Me(request);
    }
}