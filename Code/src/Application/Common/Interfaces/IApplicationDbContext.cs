using SiteManager.V4.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace SiteManager.V4.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<TodoList> TodoLists { get; set; }

        DbSet<TodoItem> TodoItems { get; set; }

        DbSet<AppLog> AppLog { get; set; }

        DbSet<AppRoleResource> AppRoleResource { get; set; }

        DbSet<AppPermissionType> AppPermissionType { get; set; }

        DbSet<AppResource> AppResource { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
