using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HRCompanyPortal.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the HRCompanyPortalUser class
    public class HRCompanyPortalUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName="nvarchar(50)")]
        public string FristName { get; set; }


        [PersonalData]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }


        [PersonalData]
        [Column(TypeName = "nvarchar(12)")]
        public string PanCard { get; set; }


    }
}
