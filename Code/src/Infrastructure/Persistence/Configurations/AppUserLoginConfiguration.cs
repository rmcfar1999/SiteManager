using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
        {
            builder.ToTable("AppUserLogin");
            builder.Property(e => e.UserId).HasColumnName("AppUserId");
        }
    }
}
