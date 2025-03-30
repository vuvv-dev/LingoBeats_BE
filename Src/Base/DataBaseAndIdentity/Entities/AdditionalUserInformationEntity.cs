using Base.DataBaseAndIdentity.Common;

namespace Base.DataBaseAndIdentity.Entities;

public class AdditionalUserInformationEntity : BaseEntity<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }

    #region Navigations
    public IdentityUserEntity IdentityUser { get; set; }
    #endregion

    public static class Metadata
    {
        public static readonly string TableName = "additional_user_information";

        public static class Properties
        {
            public static class FirstName
            {
                public const string ColumnName = "first_name";
                public const bool IsNotNull = true;
                public const short MaxLength = 255;
            }

            public static class LastName
            {
                public const string ColumnName = "last_name";
                public const bool IsNotNull = true;
                public const short MaxLength = 255;
            }

            public static class Description
            {
                public const string ColumnName = "description";
                public const bool IsNotNull = false;
                public const ushort MaxLength = ushort.MaxValue;
            }

            public static class Avatar
            {
                public const string ColumnName = "avatar";
                public const bool IsNotNull = false;
                public const ushort MaxLength = ushort.MaxValue;
            }
        }
    }
}
