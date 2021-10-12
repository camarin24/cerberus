using System;
using System.Collections.Generic;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class MeRequestDto
    {
        public string Token { get; set; }
        public Guid ApplicationId { get; set; }
    }

    public class MeResponseDto
    {
        public MeResponseDto(MeDto user, IEnumerable<string> permissions)
        {
            User = user;
            Permissions = permissions;
        }

        public MeDto User { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }


    public class MeDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Email { get; set; }
    }
}