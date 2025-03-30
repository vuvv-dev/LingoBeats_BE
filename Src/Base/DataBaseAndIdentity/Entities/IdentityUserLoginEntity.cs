using Base.DataBaseAndIdentity.Common;
using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public class IdentityUserLoginEntity : IdentityUserLogin<Guid>
{
    public static class Metadata
    {
        public const string TableName = "user_login";
    }
}
