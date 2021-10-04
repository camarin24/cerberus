using System;
using System.Collections.Generic;

namespace Intecgra.Cerberus.Domain.Dtos.Auth
{
    public class ClientDto
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<ClientApplicationDto> ClientApplications { get; set; }
        public virtual IEnumerable<UserDto> Users { get; set; }
    }
}