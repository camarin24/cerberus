using System;
using Cerberus.Infrastructure.Data.Attributes;

namespace Cerberus.Domain.Entities;

[Table("auth.application")]
public class Application
{
    [Key] public Guid ApplicationId { get; set; }
    public string Name { get; set; }
}