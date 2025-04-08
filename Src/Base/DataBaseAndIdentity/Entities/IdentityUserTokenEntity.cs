using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public sealed class IdentityUserTokenEntity : IdentityUserToken<Guid>
{
    public DateTime ExpireAt { get; set; }

    public static class Metadata
    {
        public const string TableName = "user_token";

        public static class Properties
        {
            public const string ColumnName = "expire_at";

            public const bool IsNotNull = true;
        }
    }
}
