using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable(name: "AppUser");
            builder.Property(e => e.Id).HasColumnName("AppUserId"); 
        }
    }
}
