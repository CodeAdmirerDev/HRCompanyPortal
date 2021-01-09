using HRCompanyPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRCompanyPortal.Repositories
{

   public interface IEmpRepository
    {
        IEnumerable<Employee> Employees { get; }


        public void AddEmployee(Employee emp);

        public void DeleteEmployee(Employee emp);

    }
}
