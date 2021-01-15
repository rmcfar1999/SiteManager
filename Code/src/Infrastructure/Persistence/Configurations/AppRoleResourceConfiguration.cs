using SiteManager.V4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppRoleResourceConfiguration : IEntityTypeConfiguration<AppRoleResource>
    {
        public void Configure(EntityTypeBuilder<AppRoleResource> builder)
        {
            builder.ToTable("AppRoleResource");

            builder.HasKey(e => e.AppRoleResourceId);
            builder.Property(e => e.AppRoleResourceId).ValueGeneratedOnAdd();
            builder.Property(e => e.AppPermissionTypeId).HasColumnType("int");
            builder.Property(e => e.AppRoleId).HasColumnType("int");
            builder.Property(e => e.AppResourceId).HasColumnType("int");

            builder.HasOne(x => x.AppResource)
                .WithMany(x => x.AppRoleResources)
                .HasForeignKey(x => x.AppResourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AppResource_AppRoleResouce_FK");

            builder.HasOne(x => x.AppPermissionType)
                .WithMany(x => x.AppRoleResources)
                .HasForeignKey(x => x.AppPermissionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AppResource_AppPermissionType_FK");
        }
    }
}
