using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRCompanyPortal.Models
{
    public class ModifyRole
    {

        [Required]
        public string RoleName { set; get; }

        public string RoleID { set; get; }
        public string[] AddIds  { set; get; }
        public string[] DeleteIds { set; get; }
    }
}
