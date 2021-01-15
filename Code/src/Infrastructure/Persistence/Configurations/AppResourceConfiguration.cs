using SiteManager.V4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppResourceConfiguration : IEntityTypeConfiguration<AppResource>
    {
        public void Configure(EntityTypeBuilder<AppResource> builder)
        {
            builder.ToTable("AppResource");

            builder.HasKey(e => e.AppResourceId);
            builder.Property(e => e.AppResourceId).ValueGeneratedOnAdd();
            builder.Property(e => e.ResourceRoute)
                .IsRequired()
                .HasColumnType("varchar(255)");

        }
    }
}
