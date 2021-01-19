using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HRCompanyPortal.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]

        public string FirstName { get; set; }

        public string userName { get; set; }

        public string password { get; set; }

        public string LastName { get; set; }
        public string Position { get; set; }
        public string Department { get; set; }

        public double Salary { get; set; }
        public DateTime DOJ { get; set; }
        public DateTime LastUpdated { get; set; }

        
    }
}
