using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class IdentityUserTokenEntityConfiguration
    : IEntityTypeConfiguration<IdentityUserTokenEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserTokenEntity> builder)
    {
        builder
            .Property(e => e.ExpireAt)
            .HasColumnName(IdentityUserTokenEntity.Metadata.Properties.ColumnName)
            .IsRequired();
    }
}
