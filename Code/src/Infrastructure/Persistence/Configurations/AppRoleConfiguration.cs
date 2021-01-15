using SiteManager.V4.Domain.Entities;
using SiteManager.V4.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppRoleConfiguration : IEntityTypeConfiguration<AppRole>
    {
        public void Configure(EntityTypeBuilder<AppRole> builder)
        {
            builder.ToTable(name: "AppRole");
            builder.Property(e => e.Id).HasColumnName("AppRoleId"); 
        }
    }
}
