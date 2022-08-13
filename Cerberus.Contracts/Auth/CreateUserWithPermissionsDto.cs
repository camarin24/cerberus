namespace Cerberus.Contracts.Auth;

public class CreateUserWithPermissionsContract : CreateUserContract
{
    public CreateUserWithPermissionsContract()
    {
        Permissions = new List<string>();
    }

    public IEnumerable<string> Permissions { get; set; }
    public Guid ApplicationId { get; set; }
}