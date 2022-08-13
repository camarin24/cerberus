namespace Cerberus.Contracts.Auth;

public class CreateUserContract
{
    public Guid ClientId { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}