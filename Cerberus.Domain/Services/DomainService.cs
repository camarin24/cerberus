using System;

namespace Cerberus.Domain.Services
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DomainService : Attribute
    {
    }
}