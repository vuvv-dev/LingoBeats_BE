using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class BaseIdentityRoleClaimEntityConfiguration
    : IEntityTypeConfiguration<IdentityRoleClaimEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleClaimEntity> builder)
    {
        builder.ToTable(IdentityRoleClaimEntity.Metadata.TableName);
    }
}
