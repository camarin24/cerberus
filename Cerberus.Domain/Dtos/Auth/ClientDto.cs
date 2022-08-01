using System;
using System.Collections.Generic;

namespace Cerberus.Domain.Dtos.Auth;

public class ClientDto
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = null!;
}