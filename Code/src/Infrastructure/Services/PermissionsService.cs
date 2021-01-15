using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using SiteManager.V4.Infrastructure.Identity;
using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Infrastructure.Persistence;
using SiteManager.V4.Domain.Entities;
using IdentityServer4.EntityFramework.Options;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SiteManager.V4.Infrastructure.Services
{
    public class PermissionsService : IPermissionsService
    {
        public PermissionsService( //ToDo: Inject cache data or something..drop all the below DI chains
            IConfiguration configuration,
            ICurrentUserService currentUserService,
            IOptions<OperationalStoreOptions> operationalStoreOptions,
            IDateTime dateTime)
        {

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //builder.UseSqlServer(connectionString);
            builder.UseNpgsql(connectionString);

            using (var ctx = new ApplicationDbContext(builder.Options,operationalStoreOptions,currentUserService,dateTime))
            {
                Permissions = ctx.AppRoleResource
                    .Include(x => x.AppPermissionType)
                    .Include(x => x.AppResource).ToList();
            }
        }

        public bool HasPermission(List<int> userRoles, string resource, string permissionType)
        {
            return Permissions.Where(x => userRoles.Contains(x.AppRoleId)
                && x.AppResource.ResourceRoute.ToLower() == resource.ToLower()
                && x.AppPermissionType.PermissionType.ToLower() == permissionType.ToLower()).Any();
        }
        public List<AppRoleResource> Permissions { get; set; }
        
    }
}
