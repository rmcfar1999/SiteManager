using SiteManager.V4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppPermissionTypeConfiguration : IEntityTypeConfiguration<AppPermissionType>
    {
        public void Configure(EntityTypeBuilder<AppPermissionType> builder)
        {
            builder.ToTable("AppPermissionType");

            builder.HasKey(e => e.AppPermissionTypeId);
            builder.Property(e => e.AppPermissionTypeId).ValueGeneratedOnAdd();
            builder.Property(e => e.PermissionType)
                .IsRequired()
                .HasColumnType("varchar(25)");
        }
    }
}
