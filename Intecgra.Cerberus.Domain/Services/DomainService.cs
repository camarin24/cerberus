using System;

namespace Intecgra.Cerberus.Domain.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DomainService : Attribute
    {
    }
}