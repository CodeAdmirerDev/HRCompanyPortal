using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRCompanyPortal.Areas.Identity.Data;
using HRCompanyPortal.Middlewares;
using HRCompanyPortal.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HRCompanyPortal.Repositories;
using Microsoft.Extensions.Logging;
using System.IO;
namespace HRCompanyPortal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<HRComPortalDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("HRComPortalDbContextConnection")));

         
            services.AddSingleton<IEmpRepository, EmpRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddTransient<IEmpRepository, EmpRepository>();

            //services.AddScoped<IEmpRepository, EmpRepository>();
            services.AddDistributedMemoryCache();
            services.AddSession();

            services.AddControllersWithViews();
            services.AddRazorPages();
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory factorylog)
        {

            var pathforlogfile = Directory.GetCurrentDirectory();

            factorylog.AddFile($"{pathforlogfile}\\Log\\NewLog.txt");
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            
            /*

            Middleware execution order for Use,Run,Map methods in ASP.NET core

            app.Map("/Suri", appmapsuri =>
             {


                 appmapsuri.Use(async (context, next) =>
                 {

                     await context.Response.WriteAsync("Suri befor I am from use method ");


                     await context.Response.WriteAsync(" Suri after I am from use method");

                     await next();

                 });


                 appmapsuri.Run(async (context) =>
                 {

                     await context.Response.WriteAsync("Suri2.befor I am from run method ");

                     await context.Response.WriteAsync("Suri 2.after I am from run method");


                 });



             });



            app.Run(async (context) =>
            {

                await context.Response.WriteAsync("2.befor I am from run method ");

                await context.Response.WriteAsync("2.after I am from run method");


            });


            app.Use(async (context, next) =>
            {

                await context.Response.WriteAsync("befor I am from use method ");


                await context.Response.WriteAsync("after I am from use method");

                await next();

            });

            */



            
            app.UseStaticFiles();
            app.UseSession();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseLogMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //To add area pages into routing moudules
                endpoints.MapRazorPages();
            });
        }
    }
}
