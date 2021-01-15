using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.AspNetCore.Identity;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder)
        {
            builder.ToTable("AppUserToken");
            builder.Property(e => e.UserId).HasColumnName("AppUserId");
        }
    }
}
