using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public sealed class BaseIdentityUserClaimEntity : IEntityTypeConfiguration<IdentityUserClaimEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserClaimEntity> builder)
    {
        builder.ToTable(IdentityUserClaimEntity.Metadata.TableName);
    }
}
