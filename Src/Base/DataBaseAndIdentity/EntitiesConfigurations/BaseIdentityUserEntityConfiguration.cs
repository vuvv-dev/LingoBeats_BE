using Base.DataBaseAndIdentity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Base.DataBaseAndIdentity.EntitiesConfigurations;

public sealed class BaseIdentityUserEntityConfiguration
    : IEntityTypeConfiguration<IdentityUserEntity>
{
    public void Configure(EntityTypeBuilder<IdentityUserEntity> builder)
    {
        builder.ToTable(IdentityUserEntity.Metadata.TableName);
        builder
            .HasOne(user => user.AdditionUserInformation)
            .WithOne(additionUserInformation => additionUserInformation.IdentityUser)
            .HasForeignKey<AdditionalUserInformationEntity>(additionalUserInfor =>
                additionalUserInfor.Id
            )
            .HasPrincipalKey<IdentityUserEntity>(user => user.Id)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
