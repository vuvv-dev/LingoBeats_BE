namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

using Base.DataBaseAndIdentity.Common;
using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class AdditionalUserInformationEnityConfiguration
    : IEntityTypeConfiguration<AdditionalUserInformationEntity>
{
    public void Configure(EntityTypeBuilder<AdditionalUserInformationEntity> builder)
    {
        builder.ToTable(AdditionalUserInformationEntity.Metadata.TableName);
        builder.HasKey(entity => entity.Id);

        builder
            .Property(entity => entity.FirstName)
            .HasColumnName(AdditionalUserInformationEntity.Metadata.Properties.FirstName.ColumnName)
            .HasColumnType(
                $"{Constant.DatabaseType.VARCHAR}({AdditionalUserInformationEntity.Metadata.Properties.FirstName.MaxLength})"
            )
            .IsRequired(AdditionalUserInformationEntity.Metadata.Properties.FirstName.IsNotNull);
        builder
            .Property(entity => entity.LastName)
            .HasColumnName(AdditionalUserInformationEntity.Metadata.Properties.LastName.ColumnName)
            .HasColumnType(
                $"{Constant.DatabaseType.VARCHAR}({AdditionalUserInformationEntity.Metadata.Properties.LastName.MaxLength})"
            )
            .IsRequired(AdditionalUserInformationEntity.Metadata.Properties.LastName.IsNotNull);
        builder
            .Property(entity => entity.Description)
            .HasColumnName(
                AdditionalUserInformationEntity.Metadata.Properties.Description.ColumnName
            )
            .HasColumnType(
                $"{Constant.DatabaseType.VARCHAR}({AdditionalUserInformationEntity.Metadata.Properties.Description.MaxLength})"
            )
            .IsRequired(AdditionalUserInformationEntity.Metadata.Properties.Description.IsNotNull);
        builder
            .Property(entity => entity.Avatar)
            .HasColumnName(AdditionalUserInformationEntity.Metadata.Properties.Avatar.ColumnName)
            .HasColumnType(
                $"{Constant.DatabaseType.VARCHAR}({AdditionalUserInformationEntity.Metadata.Properties.Avatar.MaxLength})"
            )
            .IsRequired(AdditionalUserInformationEntity.Metadata.Properties.Avatar.IsNotNull);
    }
}
