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
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;

namespace HRCompanyPortal.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly HRComPortalDbContext _context;

        private IEmpRepository _repo;
        private ILogger<EmployeesController> _logger;

        public IConfiguration _configuration;
        public EmployeesController(HRComPortalDbContext context,IEmpRepository repository, IConfiguration configuration
            ,ILogger<EmployeesController> logger)
        {
            _context = context;
            _configuration = configuration;
            _repo = repository;
            _logger = logger;
        }
        public string GetToken()
        {


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));

            var signin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]
                , expires: DateTime.Now.AddDays(1), signingCredentials: signin);

            return new JwtSecurityTokenHandler().WriteToken(token);

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

            var tokenString = GetToken();


            using (var httpclient =new HttpClient())
            {

                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response=await httpclient.GetAsync("http://localhost:54565/api/Employee/AllEmps"))
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
            var tokenString = GetToken();



           List<Employee> empdat = new List <Employee>();
            using (var httpclient = new HttpClient())
            {
                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenString);
                using (var response = await httpclient.GetAsync("http://localhost:54565/api/Employee/GetEmpByID/" + id))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

                    empdat = JsonConvert.DeserializeObject<List<Employee>>(empRespData);
                }


            }



            return View(empdat[0]);
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


                Employee empdata = new Employee();

                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");


                using (var httpclient = new HttpClient())
                {

                    using (var response = await httpclient.PostAsync("http://localhost:8888/api/Employee/Postemp/", content))
                    {

                        string empRespData = await response.Content.ReadAsStringAsync();

                        empdata = JsonConvert.DeserializeObject<Employee>(empRespData);


                    }


                }


                //_context.Add(employee);
                //await _context.SaveChangesAsync();

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

            //var employee = await _context.Employees.FindAsync(id);

            Employee employee = new Employee();
            using (var httpclient = new HttpClient())
            {

                using (var response = await httpclient.GetAsync("http://localhost:8888/api/Employee/GetEmpByID/" + id))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

                    employee = JsonConvert.DeserializeObject<Employee>(empRespData);
                }


            }


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


                    StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");


                    using (var httpclient = new HttpClient())
                    {

                        using (var response = await httpclient.PutAsync("http://localhost:8888/api/Employee/PutEmp/"+id, content))
                        {

                            string empRespData = await response.Content.ReadAsStringAsync();



                        }


                    }


                    //_context.Update(employee);
                    //await _context.SaveChangesAsync();




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



            Employee employee = new Employee();
            using (var httpclient = new HttpClient())
            {

                using (var response = await httpclient.GetAsync("http://localhost:8888/api/Employee/GetEmpByID/" + id))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

                    employee = JsonConvert.DeserializeObject<Employee>(empRespData);
                }


            }


            //var employee = await _context.Employees
            //    .FirstOrDefaultAsync(m => m.EmployeeId == id);



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
        //    var employee = await _context.Employees.FindAsync(id);
        //    _context.Employees.Remove(employee);
        //    await _context.SaveChangesAsync();




        Employee employee = new Employee();
            using (var httpclient = new HttpClient())
            {

                using (var response = await httpclient.DeleteAsync("http://localhost:8888/api/Employee/DeleteempId/" + id))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

    employee = JsonConvert.DeserializeObject<Employee>(empRespData);
                }


            }


            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
