using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Cerberus.Contracts.Auth;
using Cerberus.Domain.Dtos.Auth;
using Cerberus.Domain.Entities;
using Cerberus.Domain.Exceptions;
using Cerberus.Domain.Ports.Auth;
using Cerberus.Domain.Ports.Repository;
using Cerberus.Domain.Utilities;
using Cerberus.Infrastructure.Extensions;
using Cerberus.Infrastructure.Utils;
using Cerberus.Infrastructure.Utils.Ports;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Cerberus.Domain.Services.Auth;

[DomainService]
public class UserService : BaseService<User, UserDto>, IUserService
{
    private readonly IClientService _clientService;
    private readonly IConfiguration _configuration;
    private readonly IMessagesManager _messagesManager;
    private readonly IPermissionService _permissionService;
    private readonly IGenericRepository<User> _repository;
    private readonly IUserPermissionService _userPermissionService;
    private readonly IFileManager _fileManager;

    public UserService(IGenericRepository<User> repository, IMessagesManager messagesManager,
        IClientService clientService, IConfiguration configuration, IPermissionService permissionService,
        IUserPermissionService userPermissionService, IFileManager fileManager) : base(
        repository)
    {
        _repository = repository;
        _messagesManager = messagesManager;
        _clientService = clientService;
        _configuration = configuration;
        _permissionService = permissionService;
        _userPermissionService = userPermissionService;
        _fileManager = fileManager;
    }

    public async Task<UserDto> CreateUser(UserDto dto)
    {
        dto.Salt = PasswordHash.CreateSalt();
        dto.Picture = await _fileManager.UploadProfilePicture(dto.Name);
        dto.Password = PasswordHash.Create(dto.Password, dto.Salt);
        var userId = await Create<Guid>(dto);
        return await GetById(userId);
    }

    public async Task<AuthorizationContract> Login(LoginDto request)
    {
        var users =
            (await _repository.Where(new {request.Email})).ToList();
        if (users == null || !users.Any()) throw new DomainException(_messagesManager.GetMessage("InvalidUser"));
        var user = users.FirstOrDefault(m => PasswordHash.Validate(request.Password, m.Password, m.Salt));
        if (user == null) throw new DomainException(_messagesManager.GetMessage("InvalidPassword"));

        // Hacemos esto para asegurarnos que el cliente tiene acceso a la app solicitada
        await _clientService.GetClientByAppIdAndId(request.ApplicationId, user.ClientId);
        var permissions =
            await _permissionService.GetPermissionsByApplicationAndUser(request.ApplicationId, user.UserId);
        return GenerateAuthorizationDto(user.Adapt<UserDto>(), permissions);
    }

    public async Task<MeResponseContract> Me(MeRequestContract request)
    {
        var claims = ValidateToken(request.Token);
        var user = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var permissions = await _permissionService.GetPermissionsByApplicationAndUser(request.ApplicationId, user);
        var me = (await GetById(user)).Adapt<MeContract>();
        return new MeResponseContract(me, permissions.Select(m => m.Name));
    }

    public async Task<AuthorizationContract> RefreshToken(MeRequestContract request)
    {
        var claims = ValidateToken(request.Token);
        var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var permissions =
            await _permissionService.GetPermissionsByApplicationAndUser(request.ApplicationId, userId);
        var user = await GetById(userId);
        return GenerateAuthorizationDto(user, permissions);
    }

    public async Task<UserDto> CreateUserWithPermissions(CreateUserWithPermissionsContract request)
    {
        var user = await CreateUser(new UserDto
        {
            Email = request.Email,
            Name = request.Name,
            Password = request.Password,
            ClientId = request.ClientId
        });
        var permissions = await _permissionService.GetPermissionsByName(request.ApplicationId, request.Permissions);
        if (permissions.Any())
            await _userPermissionService.Create(permissions.Select(p => new UserPermissionDto
            {
                PermissionId = p.PermissionId,
                UserId = user.UserId
            }));

        return user;
    }

    private AuthorizationContract GenerateAuthorizationDto(UserDto user, IEnumerable<PermissionDto> permissions)
    {
        var expiration = DateTime.UtcNow.AddDays(_configuration.GetSectionVale("Authorization:TokenExpirationTime"));
        return new AuthorizationContract(GenerateJwtToken(user, expiration),
            user.Name, user.Picture, user.Email, permissions.Select(m => m.Name),
            GenerateJwtRefreshToken(user), expiration, user.UserId, user.ClientId);
    }

    private string GenerateJwtToken(UserDto user, DateTime expiration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSectionValue("Authorization:Secret"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = expiration,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateJwtRefreshToken(UserDto user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration.GetSectionValue("Authorization:Secret"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }),
            Expires = DateTime.UtcNow.AddDays(
                _configuration.GetSectionVale("Authorization:RefreshTokenExpirationTime")),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private ClaimsPrincipal ValidateToken(string token)
    {
        var key = Encoding.UTF8.GetBytes(_configuration.GetSectionValue("Authorization:Secret"));
        var handler = new JwtSecurityTokenHandler();
        var validations = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        var claims = handler.ValidateToken(token, validations, out var tokenSecure);
        return claims;
    }
}