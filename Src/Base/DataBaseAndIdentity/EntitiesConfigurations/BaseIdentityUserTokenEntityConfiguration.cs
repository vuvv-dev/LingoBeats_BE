using Base.DataBaseAndIdentity.Common;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public class BaseIdentityUserTokenEntityConfiguration
    : IEntityTypeConfiguration<IdentityUserTokenEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserTokenEntity> builder)
    {
        builder.ToTable(IdentityUserTokenEntity.Metadata.TableName);
        builder
            .Property(entity => entity.ExpireAt)
            .HasColumnName(IdentityUserTokenEntity.Metadata.Properties.ColumnName)
            .HasColumnType(Constant.DatabaseType.TIMESTAMPZ)
            .IsRequired(IdentityUserTokenEntity.Metadata.Properties.IsNotNull);
    }
}
