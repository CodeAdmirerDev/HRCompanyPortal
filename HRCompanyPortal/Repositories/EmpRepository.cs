using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCompanyPortal.Models;

namespace HRCompanyPortal.Repositories
{
    public class EmpRepository : IEmpRepository
    {
        List<Employee> emps =new List<Employee>();
        public IEnumerable<Employee> Employees => emps;

        public EmpRepository()
        {
            new List<Employee> {
                new Employee{FirstName="Suri",LastName="L",Department="IT",
                    Salary=12522,EmployeeId=252,DOJ=DateTime.Now,LastUpdated=DateTime.Now,Position="SSE"},
                   new Employee{FirstName="Amar",LastName="T",Department="IT",
                       Salary=12522,EmployeeId=252,DOJ=DateTime.Now,LastUpdated=DateTime.Now,Position="TL"}
            }.ForEach(emp => AddEmployee(emp));
        }

        
        public void AddEmployee(Employee emp)
        {
            emps.Add( emp);
            
        }

        public void DeleteEmployee(Employee emp)
        {

            emps.Remove(emp);
        }


    }
}
