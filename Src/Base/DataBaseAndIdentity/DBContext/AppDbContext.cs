using Base.DataBaseAndIdentity.Common;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Base.DataBaseAndIdentity.DBContext;

public sealed class AppDbContext : IdentityDbContext<IdentityUserEntity, IdentityRoleEntity, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        builder.HasDefaultSchema(Constant.DatabaseSchema);
        InitCaseInsensitiveCollation(builder);
    }

    private static void InitCaseInsensitiveCollation(ModelBuilder builder)
    {
        builder.HasCollation(Constant.Collation.CASE_INSENSITIVE, "en-u-ks-primary", "icu", false);
    }
}
