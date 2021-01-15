﻿using SiteManager.V4.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace SiteManager.V4.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}