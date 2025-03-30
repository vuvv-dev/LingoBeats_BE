using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class BaseIdentityUserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityRoleEntity>
{
    public void Configure(EntityTypeBuilder<IdentityRoleEntity> builder)
    {
        builder.ToTable(IdentityRoleEntity.Metadata.TableName);
    }
}
