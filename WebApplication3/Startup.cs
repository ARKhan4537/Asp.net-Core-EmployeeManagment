using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using WebApplication3.Models;
using WebApplication3.Security;

namespace WebApplication3
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;

        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));
            services.AddMvc(
            option =>
            {
                option.EnableEndpointRouting = false;
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                option.Filters.Add(new AuthorizeFilter(policy));
               
            }
           

            ).AddXmlSerializerFormatters();

            //services.AddMvc().AddRazorPagesOptions(options =>
            //{
            //    options.Conventions.AddPageRoute("/Home/DefaultPage", "");
            //});

            services.AddAuthentication().AddGoogle(options =>
            {
                options.ClientId = "686991842947-mfkg0o9polv9d6mmp041tfppa99mfqb2.apps.googleusercontent.com";
                options.ClientSecret = "Pr_trQfbvE8KT7QHeCKhPgWi";
            });


            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            //policy Variable 
            services.AddAuthorization(options =>
            {

                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role"));

                options.AddPolicy("EditRolePolicy", policy =>

                policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("EditRolePolicy", policy =>

                policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));

                options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin,Super Admin"));
                

            });
            //services.AddRazorPages();
            services.AddScoped<EmployeeRepository, SQLEmployeeRepository>();
            services.AddSingleton<IAuthorizationHandler, CanEditOnlyOtherAdminRolesAndClaimsHandler>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
            services.AddSingleton<DataProtectionPurposeStrings>();
            services.AddIdentity<ApplicationUser, IdentityRole>(


                options =>
                {
                    options.Password.RequiredLength = 10;
                    options.Password.RequiredUniqueChars = 3;
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

                })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(5));




            // services.Configure<IdentityOptions>(options =>
            // {
            //     options.Password.RequiredLength = 10;
            //     options.Password.RequiredUniqueChars = 3;
            // });
            //services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }



            // FileServerOptions fileServerOptions = new FileServerOptions();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            // fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("default.html");
            // app.UseFileServer(fileServerOptions);
            // app.UseStaticFiles();

            //app.UseDefaultFiles();

            //app.UseRouting();
            //app.UseStaticFiles();
            app.UseAuthentication();
            app.UseStaticFiles();
            
            //Routing Settings

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=DefaultPage}/{id?}");

            });

          //  app.UseEndpoints(endpoints =>
           // {
           //     endpoints.MapControllerRoute(
             //       name: "default",
             //       pattern: "{controller=Home}/{action=Index}/{id?}");
            //});


            //app.UseMvcWithDefaultRoute();


            //app.Run(async (context) =>
            //{
            //   await context.Response.WriteAsync("Hello World..");
            // });





           // app.UseEndpoints(endpoints =>
           // {
           // endpoints.MapGet("/", async context =>
           // {
             // throw new Exception("Some Error Processing the request");
               // await context.Response.WriteAsync("Hello World..!");

             //});
        //    });
        }
    }
}