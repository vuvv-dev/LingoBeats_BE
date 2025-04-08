using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class IdentityRoleEntity : IdentityRole<Guid>
{
    public static class Metadata
    {
        public const string TableName = "role";
    }
}
