using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder)
        {
            builder.ToTable(name: "AppUserClaim");
            builder.Property(e => e.UserId).HasColumnName("AppUserId");
            builder.Property(e => e.Id).HasColumnName("AppUserClaimId");
        }
    }
}
