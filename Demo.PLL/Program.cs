using Demo.BLL.Interfaces;
using Demo.BLL.Repositry;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Demo.PLL.MappingProfiles;
using Demo.PLL.Services.EmailSender;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PLL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var webApplicationBuilder = WebApplication.CreateBuilder(args);
			#region ConfigureServices
			webApplicationBuilder.Services.AddControllersWithViews();
			webApplicationBuilder.Services.AddDbContext<MVCAppContext>(options =>
			options.UseSqlServer(webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection")));
			webApplicationBuilder.Services.AddScoped<IDepartmentRepositry, DepartmentRepositry>();
			webApplicationBuilder.Services.AddTransient<IEmailSender, EmailSender>();
			webApplicationBuilder.Services.AddScoped<IUnitWork, UnitOfWork>();
			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
			webApplicationBuilder.Services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile()));
			webApplicationBuilder.Services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddEntityFrameworkStores<MVCAppContext>().AddDefaultTokenProviders();
			webApplicationBuilder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
			{
				options.LoginPath = "Account/Login";
				options.AccessDeniedPath = "Home/Error";
			});
			#endregion
			var app = webApplicationBuilder.Build();
			#region Configure Kesterel MiddelWare
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
					pattern: "{controller=Account}/{action=Login}/{id?}");
			});
			#endregion
			app.Run();
		}
	
		
	}
}
