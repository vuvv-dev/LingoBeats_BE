using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class IdentityUserRoleEntity : IdentityUserRole<Guid>
{
    public static class Metadata
    {
        public const string TableName = "user_role";
    }
}
