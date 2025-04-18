﻿namespace Base.Config;

public sealed class AspNetCoreIdentityOption
{
    public PasswordOption? Password { get; init; }
    public LockoutOption? Lockout { get; init; }
    public UserOption? User { get; init; }
    public SignInOption? SignIn { get; init; }

    public sealed class PasswordOption
    {
        public bool RequireDigit { get; init; }
        public bool RequireLowerCase { get; init; }
        public bool RequireNonAlphanumeric { get; init; }
        public bool RequireUpperCase { get; init; }
        public int RequiredLength { get; init; }
        public int RequiredUniqueChars { get; init; }
    }

    public sealed class LockoutOption
    {
        public int DefaultLockoutTimeSpanInSeconds { get; init; }
        public int MaxFailedAccessAttempts { get; init; }
        public bool AllowedForNewUsers { get; init; }
    }

    public sealed class UserOption
    {
        public string AllowedUserNameCharacters { get; init; }
        public bool RequireUniqueEmail { get; init; }
    }

    public sealed class SignInOption
    {
        public bool RequireConfirmedEmail { get; init; }
        public bool RequireConfirmedPhoneNumber { get; init; }
    }
}
