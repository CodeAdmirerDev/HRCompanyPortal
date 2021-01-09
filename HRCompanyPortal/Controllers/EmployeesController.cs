using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRCompanyPortal.Models;
using HRCompanyPortal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using HRCompanyPortal.Extenstions;
using System.Net.Http;
using Newtonsoft.Json;

namespace HRCompanyPortal.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HRComPortalDbContext _context;

        private IEmpRepository _repo;
        private ILogger<EmployeesController> _logger;
        public EmployeesController(HRComPortalDbContext context,IEmpRepository repository,
            ILogger<EmployeesController> logger)
        {
            _context = context;

            _repo = repository;
            _logger = logger;
        }


        public ActionResult ClearSeesion()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

            // GET: Employees
        [Authorize(Roles = "Admin,HRManager,HREmployee")]
        public async Task<IActionResult> Index()
        {
            //  return View(await _context.Employees.ToListAsync());

            _logger.LogInformation("I am in the index method of employee");


            List<Employee> emplist = new List<Employee>();

            using (var httpclient =new HttpClient())
            {

                using (var response=await httpclient.GetAsync("http://localhost:8888/api/Employee/GetEmp"))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

                    emplist = JsonConvert.DeserializeObject<List<Employee>>(empRespData);
                }


            }



                ViewBag.EMPName = "Suri";
            ViewData["empn"] = "Vinay";
            TempData["Empna"] = "Amar";

            HttpContext.Session.SetString("Empnaaa", "Phani");

            HttpContext.Session.SetObject("suri", emplist);

           // return RedirectToAction("Index","Home");
            return View(emplist);
        }


        [Authorize(Roles = "Admin,HRManager,HREmployee")]
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var employee = await _context.Employees
            //    .FirstOrDefaultAsync(m => m.EmployeeId == id);
            //if (employee == null)
            //{
            //    return NotFound();
            //}


            Employee empdat = new Employee();
            using (var httpclient = new HttpClient())
            {

                using (var response = await httpclient.GetAsync("http://localhost:8888/api/Employee/"+id))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

                    empdat = JsonConvert.DeserializeObject<Employee>(empRespData);
                }


            }



            return View(empdat);
        }

        // GET: Employees/Create
        [Authorize(Roles = "HRManager")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "HRManager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EmployeeId,FirstName,LastName,Position,Department,Salary,DOJ,LastUpdated")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();

                TempData["SucessMsg"] = "Employee record created sucessfully !";
                return RedirectToAction(nameof(Index));
            
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "HRManager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            TempData["PageTitle"] = "Employee eidt page";

            TempData["PageHeader"] = "Employee eidt";

            TempData["EmployeeModel"] = employee;

            
            return View(employee);
        }



        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "HRManager")]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,FirstName,LastName,Position,Department,Salary,DOJ,LastUpdated")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "HRManager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [Authorize(Roles = "HRManager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
