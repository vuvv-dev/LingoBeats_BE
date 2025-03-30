using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public class IdentityRoleClaimEntity : IdentityRoleClaim<Guid>
{
    public static class Metadata
    {
        public const string TableName = "role_claim";
    }
}
