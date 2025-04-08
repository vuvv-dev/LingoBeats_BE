namespace SignIn.Models;

public sealed class PasswordSignInResultModel
{
    public bool IsSuccess { get; set; }
    public bool IsLockedOut { get; set; }

    public Guid UserId { get; set; }
}
