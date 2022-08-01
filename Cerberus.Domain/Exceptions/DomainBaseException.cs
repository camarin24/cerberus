using System;

namespace Cerberus.Domain.Exceptions;

[Serializable]
public class DomainBaseException : Exception
{
    public DomainBaseException(string message) : base(message)
    {
    }
}