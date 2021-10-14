using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Intecgra.Cerberus.Domain.Dtos.Auth;
using Intecgra.Cerberus.Domain.Entities;
using Intecgra.Cerberus.Domain.Exceptions;
using Intecgra.Cerberus.Domain.Ports.Auth;
using Intecgra.Cerberus.Domain.Ports.Data;
using Intecgra.Cerberus.Domain.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Intecgra.Cerberus.Domain.Services.Auth
{
    [DomainService]
    public class UserService : BaseService<User, UserDto>, IUserService
    {
        private readonly IGenericRepository<User> _repository;
        private readonly IMapper _mapper;
        private readonly IMessagesManager _messagesManager;
        private readonly IClientService _clientService;
        private readonly IConfiguration _configuration;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<UserService> _logger;

        public UserService(IGenericRepository<User> repository, IMapper mapper, IMessagesManager messagesManager,
            IClientService clientService, IConfiguration configuration, IPermissionService permissionService,
            ILogger<UserService> logger) : base(
            repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _messagesManager = messagesManager;
            _clientService = clientService;
            _configuration = configuration;
            _permissionService = permissionService;
            _logger = logger;
        }

        public new async Task<UserDto> Create(UserDto dto)
        {
            dto.Salt = PasswordHash.CreateSalt();
            dto.Password = PasswordHash.Create(dto.Password, dto.Salt);
            return await Save(dto);
        }

        public async Task<AuthorizationDto> Login(LoginDto request)
        {
            var users = (await GetFilter(u => u.Email.Equals(request.Email))).ToList();
            if (users == null || !users.Any()) throw new DomainException(_messagesManager.GetMessage("InvalidUser"));
            var user = users.FirstOrDefault(m => PasswordHash.Validate(request.Password, m.Password, m.Salt));
            if (user == null) throw new DomainException(_messagesManager.GetMessage("InvalidPassword"));

            var client = await _clientService.GetClientByAppIdAndId(request.AppId, user.ClientId);
            var permissions = await _permissionService.GetPermissionsByApplicationAndUser(request.AppId, user.UserId);

            return GenerateAuthorizationDto(user, permissions);
        }

        private AuthorizationDto GenerateAuthorizationDto(UserDto user, IEnumerable<PermissionDto> permissions)
        {
            var expiration = DateTime.UtcNow.AddSeconds(20);
            return new AuthorizationDto(GenerateJwtToken(user, expiration),
                user.Name, user.Picture, user.Email, permissions.Select(m => m.Name),
                GenerateJwtRefreshToken(user), expiration);
        }

        public async Task<IEnumerable<UserDto>> GetFilter(Expression<Func<User, bool>> filter)
        {
            return _mapper.Map<IEnumerable<UserDto>>(await _repository.Get(filter));
        }

        public async Task<MeResponseDto> Me(MeRequestDto request)
        {
            var claims = ValidateToken(request.Token);
            var user = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var permissions = await _permissionService.GetPermissionsByApplicationAndUser(request.ApplicationId, user);
            var me = _mapper.Map<MeDto>(await GetById(user));
            return new MeResponseDto(me, permissions.Select(m => m.Name));
        }

        public async Task<AuthorizationDto> RefreshToken(MeRequestDto request)
        {
            var claims = ValidateToken(request.Token);
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var permissions =
                await _permissionService.GetPermissionsByApplicationAndUser(request.ApplicationId, userId);
            var user = await GetById(userId);
            return GenerateAuthorizationDto(user, permissions);
        }


        private string GenerateJwtToken(UserDto user, DateTime expiration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authorization:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        private string GenerateJwtRefreshToken(UserDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authorization:Secret").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("Authorization:Secret").Value);
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
}