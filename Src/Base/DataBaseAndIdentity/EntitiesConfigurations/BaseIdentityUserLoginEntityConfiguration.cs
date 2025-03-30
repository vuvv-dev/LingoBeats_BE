using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class BaseIdentityUserLoginEntityConfiguration
    : IEntityTypeConfiguration<IdentityUserLoginEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserLoginEntity> builder)
    {
        builder.ToTable(IdentityUserLoginEntity.Metadata.TableName);
    }
}
