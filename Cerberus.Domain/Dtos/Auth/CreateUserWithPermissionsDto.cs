using System;
using System.Collections.Generic;

namespace Cerberus.Domain.Dtos.Auth;

public class CreateUserWithPermissionsDto : CreateUserDto
{
    public CreateUserWithPermissionsDto()
    {
        Permissions = new List<string>();
    }

    public IEnumerable<string> Permissions { get; set; }
    public Guid ApplicationId { get; set; }
}