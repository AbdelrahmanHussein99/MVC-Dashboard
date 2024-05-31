using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC.BLL.Interfaces;
using MVC.BLL.Repositories;
using MVC.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MVC.PL.Helper;
using MVC.DAL.Data;
using Microsoft.EntityFrameworkCore;
using MVC.DAL.Models;

namespace MVC.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

            #region Configure Services
            Builder.Services.AddControllersWithViews();
            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

            Builder.Services.AddAutoMapper(M => M.AddProfile(new MappingProfiles()));
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();


            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            Builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefautConntection"));
            });

            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

            Builder.Services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Account/SignIn";
            }); 
            #endregion

            var app =Builder.Build();

            #region Pipeline

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #endregion

            app.Run();
        }

       
    }
}
