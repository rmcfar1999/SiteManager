
using SiteManager.V4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SiteManager.V4.Infrastructure.Persistence.Configurations
{
    public class TodoListConfiguration : IEntityTypeConfiguration<TodoList>
    {
        public void Configure(EntityTypeBuilder<TodoList> builder)
        {
            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
