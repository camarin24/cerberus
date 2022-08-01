using System.ComponentModel.DataAnnotations;

namespace Cerberus.Infrastructure.Data.Attributes;

public class Table : ValidationAttribute
{
    public Table(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}