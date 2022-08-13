namespace Cerberus.Contracts.Auth;

public class MeRequestContract
{
    public string Token { get; set; } = null!;
    public Guid ApplicationId { get; set; }
}

public class MeResponseContract
{
    public MeResponseContract(MeContract user, IEnumerable<string> permissions)
    {
        User = user;
        Permissions = permissions;
    }

    public MeContract User { get; set; }
    public IEnumerable<string> Permissions { get; set; }
}

public class MeContract
{
    public Guid UserId { get; set; }
    public string Name { get; set; } = null!;
    public string Picture { get; set; } = null!;
    public string Email { get; set; } = null!;
    public Guid ClientId { get; set; }
}