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




                services.AddDefaultIdentity<HRCompanyPortalUser>(options =>
                {

                    options.Password.RequiredLength = 7;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.SignIn.RequireConfirmedAccount = true;

                })
                    .AddEntityFrameworkStores<HRCompanyPortalContext>();
                
            });
        }
    }
}