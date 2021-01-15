using SiteManager.V4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class AppLogConfiguration : IEntityTypeConfiguration<AppLog>
    {
        public void Configure(EntityTypeBuilder<AppLog> builder)
        {
            builder.ToTable("AppLog");

            builder.HasKey(e => e.AppLogId);
            builder.Property(e => e.AppLogId).ValueGeneratedOnAdd();
            builder.Property(e => e.LogDateTime).HasColumnType("timestamp(7)");
            builder.Property(e => e.LogLevel).HasColumnType("text");
            builder.Property(e => e.Category).HasColumnType("text");
            builder.Property(e => e.EventId).HasColumnType("text");
            builder.Property(e => e.StateInfo).HasColumnType("text");
            builder.Property(e => e.LogMessage).HasColumnType("text");
            builder.Property(e => e.LogException).HasColumnType("text");
            builder.Property(e => e.UserName).HasColumnType("varchar(255)");
            builder.Property(e => e.JsonData).HasColumnType("json");
        }
    }
}
