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

namespace SiteManager.V4.Application.UserAdmin.Queries
{
    public class GetAllUsersQuery : IRequest<IEnumerable<AppUserDto>>
    {

    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<AppUserDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;

        public GetAllUsersQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _identityService = identityService;
        }

        public async Task<IEnumerable<AppUserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var r = await _identityService.GetAllUsers();

            return r.ToList();
        }
    }
}
