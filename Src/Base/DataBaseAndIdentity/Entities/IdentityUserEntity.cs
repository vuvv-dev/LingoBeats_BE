using Microsoft.AspNetCore.Identity;

namespace Base.DataBaseAndIdentity.Entities;

public class IdentityUserEntity : IdentityUser<Guid>
{
    #region Navigations
    public AdditionalUserInformationEntity AdditionUserInformation { get; set; }
    #endregion

    public static class Metadata
    {
        public const string TableName = "user";
    }
}
