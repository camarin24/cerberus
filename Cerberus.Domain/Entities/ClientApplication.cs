#nullable disable

using System;
using Cerberus.Infrastructure.Data.Attributes;

namespace Cerberus.Domain.Entities;

[Table("auth.client_application")]
public class ClientApplication
{
    [Key] public int ClientApplicationId { get; set; }
    public Guid ApplicationId { get; set; }
    public Guid ClientId { get; set; }
}