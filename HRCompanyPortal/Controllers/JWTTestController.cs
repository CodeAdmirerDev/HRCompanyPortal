using HRCompanyPortal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace HRCompanyPortal.Controllers
{
    public class JWTTestController : Controller
    {


        public IConfiguration _configuration;


        public JWTTestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [Authorize(Roles = "Admin,HRManager,HREmployee")]
        public async Task<IActionResult> Login(Employee employee)
        {

            var JwTtoken = "";//GetToken();

            Employee empdata = new Employee();


            empdata.userName = "Amar";
            empdata.password = "Amar123";
            empdata.EmployeeId = 1;

            StringContent content = new StringContent(JsonConvert.SerializeObject(employee), 
                Encoding.UTF8, "application/json");


            using (var httpclient = new HttpClient())
            {

                httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("HRToken", JwTtoken);

                using (var response = await httpclient.PostAsync("http://localhost:8888/api/Token/ValidateUser/", content))
                {

                    string empRespData = await response.Content.ReadAsStringAsync();

                    empdata = JsonConvert.DeserializeObject<Employee>(empRespData);


                }


                return RedirectToAction("Index", "Home");
            }
        }


      
    }
}
