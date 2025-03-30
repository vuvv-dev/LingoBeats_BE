using Base.DataBaseAndIdentity.Common;
using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class IdentityUserClaimEntity : IdentityUserClaim<Guid>
{
    public static class Metadata
    {
        public const string TableName = "user_claim";
    }
}
