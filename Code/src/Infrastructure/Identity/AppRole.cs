using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SiteManager.V4.Infrastructure.Identity
{
    public class AppRole : IdentityRole<int>
    {
        public AppRole() { }

        public AppRole(string name) : this()
        {
            this.Name = name;
        }
        //public string ShowingItsPossible { get; set; }
    }
}
