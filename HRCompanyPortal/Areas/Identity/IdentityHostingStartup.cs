using System;
using HRCompanyPortal.Areas.Identity.Data;
using HRCompanyPortal.Data;
using HRCompanyPortal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(HRCompanyPortal.Areas.Identity.IdentityHostingStartup))]
namespace HRCompanyPortal.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<HRCompanyPortalContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("HRCompanyPortalContextConnection")));
               
                
                services.AddIdentity<HRCompanyPortalUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                }).AddEntityFrameworkStores<HRCompanyPortalContext>();

                services.ConfigureApplicationCookie(opts => opts.AccessDeniedPath = "/Account/Accessdenined");


            });
        }
    }
}