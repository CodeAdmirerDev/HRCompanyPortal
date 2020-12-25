using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HRCompanyPortal.Controllers
{
    public class RoleController : Controller
    {


        private RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleMgr)
        {
            roleManager = roleMgr;

        }

        public IActionResult Index()
        {

            return View(roleManager.Roles);
        }

        private void Erros(IdentityResult result)
        {

            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("",error.Description);
        }


        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Required]string name)
        {

            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Erros(result);
            }

            return View(name);

        }





    }
}
