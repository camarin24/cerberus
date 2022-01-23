using System;
using System.Collections.Generic;

namespace Cerberus.Domain.Dtos.Auth;

public class CreateUserWithPermissionsDto : CreateUserDto
{
    public IEnumerable<string> Permissions { get; set; }
    public Guid ApplicationId { get; set; }
}