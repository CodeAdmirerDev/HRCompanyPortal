using HRCompanyPortal.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRCompanyPortal.Models
{
    public class EditRole
    {

        public IdentityRole Role { get; set; }

        public IEnumerable<HRCompanyPortalUser> Members { get; set; }
        public IEnumerable<HRCompanyPortalUser> NonMembers { get; set; }


    }
}
