using AutoMapper;
using AutoMapper.QueryableExtensions;
using SiteManager.V4.Application.Common.Models;
using SiteManager.V4.Application.Common.Interfaces;
using SiteManager.V4.Application.UserAdmin.Models;
using SiteManager.V4.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SiteManager.V4.Application.RoleAdmin.Queries
{
    public class GetAllRolesQuery : IRequest<IEnumerable<AppRoleDto>>
    {

    }

    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<AppRoleDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetAllRolesQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<IEnumerable<AppRoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var r = await _identityService.GetAllRolesAsync();

            return r;
        }
    }
}
