using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRCompanyPortal.Models
{
    public class HRComPortalDbContext :DbContext
    {


        public HRComPortalDbContext(DbContextOptions<HRComPortalDbContext> options) 
        : base(options)
        {

         
        }

        
        //Add your model classes
        public DbSet<Employee> Employees { get; set; }


    }
}
