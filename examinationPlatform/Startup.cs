using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using examinationPlatform.Common.filter;
using examinationPlatform.Interface;
using examinationPlatform.Models;
using examinationPlatform.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Log4Net;

namespace examinationPlatform
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
            services.AddControllersWithViews();
            services.AddMvc(options => { options.Filters.Add<ExceptionFilter>(); });
            services.AddSession();
            services.AddHttpContextAccessor();
            services.AddScoped<IExam_Platform_Context, exam_platformContext>();
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ITestStorage, TestService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IUserService, UserService>();
            services.AddMemoryCache();
            services.AddDbContext<exam_platformContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
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
            loggerFactory.AddLog4Net();
            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=admin}/{action=login}/{id?}");
            });
        }
    }
}
