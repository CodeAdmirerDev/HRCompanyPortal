using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using HRCompanyPortal.Areas.Identity.Data;
using HRCompanyPortal.Models;
using Microsoft.AspNetCore.Authorization;

namespace HRCompanyPortal.Controllers
{
    
    public class RoleController : Controller
    {


        private RoleManager<IdentityRole> roleManager;
        private UserManager<HRCompanyPortalUser> userManager;

        public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<HRCompanyPortalUser> userMgr)
        {
            roleManager = roleMgr;
            userManager = userMgr;

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


        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            IdentityRole role = await roleManager.FindByIdAsync(id);

            if (role!=null)
            {

            if (ModelState.IsValid)
            {

                IdentityResult result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Erros(result);
            }

            }
            else
            {
                ModelState.AddModelError("", "No role found");

            }
            return View("Index",roleManager.Roles);

        }

        public async Task<IActionResult> Update(string id)
        {

            IdentityRole role = await roleManager.FindByIdAsync(id);

            List<HRCompanyPortalUser> members = new List<HRCompanyPortalUser>();
            List<HRCompanyPortalUser> Nonmembers = new List<HRCompanyPortalUser>();


            foreach (HRCompanyPortalUser user in userManager.Users)
            {

                var list = await userManager.IsInRoleAsync(user, role.Name) ? members : Nonmembers;

                list.Add(user);
            }


            return View(new EditRole
            {

                Role = role,
                Members=members,
                NonMembers=Nonmembers
            }); 

        }

        [HttpPost]
        public async Task<IActionResult> Update(ModifyRole modifyRole)
        {
            IdentityResult result;

            if (ModelState.IsValid)
            {

                foreach(string userId in modifyRole.AddIds?? new string[] { })
                {

                    HRCompanyPortalUser user =await userManager.FindByIdAsync(userId);
                    result = await userManager.AddToRoleAsync(user, modifyRole.RoleName);

                    if (!result.Succeeded)
                        Erros(result);
                }




                foreach (string userId in modifyRole.DeleteIds ?? new string[] { })
                {

                    HRCompanyPortalUser user = await userManager.FindByIdAsync(userId);
                    result = await userManager.RemoveFromRoleAsync(user, modifyRole.RoleName);

                    if (!result.Succeeded)
                        Erros(result);
                }
            }

            if (ModelState.IsValid)
                return RedirectToAction(nameof(Index));

            else
                return View(modifyRole.RoleID);

        }//Update post


        }
    }
