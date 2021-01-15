using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
        {
            builder.ToTable(name: "AppRoleClaim");
            builder.Property(e => e.Id).HasColumnName("AppRoleClaimId");
            builder.Property(e => e.RoleId).HasColumnName("AppRoleId");
        }
    }
}
