using System;
using System.Collections.Generic;

namespace Cerberus.Domain.Dtos.Auth;

public class AuthorizationDto
{
    public AuthorizationDto(string token, string name, string image, string email, IEnumerable<string> permissions,
        string refreshToken, DateTime expireIn, Guid userId, Guid clientId)
    {
        Token = token;
        Name = name;
        Image = image;
        Email = email;
        Permissions = permissions;
        RefreshToken = refreshToken;
        ExpireIn = expireIn;
        UserId = userId;
        ClientId = clientId;
    }

    public string Name { get; set; }
    public string Image { get; set; }
    public Guid UserId { get; set; }
    public Guid ClientId { get; set; }
    public string Email { get; set; }
    public string Token { get; set; }

    public string RefreshToken { get; set; }
    public DateTime ExpireIn { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}