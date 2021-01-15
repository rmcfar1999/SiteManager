using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            builder.ToTable("AppUserRole");
            builder.Property(e => e.UserId).HasColumnName("AppUserId");
            builder.Property(e => e.RoleId).HasColumnName("AppRoleId");
        }
    }
}
