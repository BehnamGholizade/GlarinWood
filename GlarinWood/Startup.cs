using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using GlarinWood.Data;
using GlarinWood.Models;
using GlarinWood.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace GlarinWood
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services/*,IHostingEnvironment _hostingEnvironment*/)
        {
            // Add framework services.
            //جهت عملیات Skip offset .UseRowNumberForPaging()
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.UseRowNumberForPaging()));
            //سفارش سازی اعتبارسنجی
            //AddIdentity فرایند ثبت httpcontext را انجام می دهد
            services.AddIdentity<ApplicationUser, IdentityRole>(ops =>
            {
                // User settings
                ops.User.RequireUniqueEmail = true;
                //جهت تنظیمات گذرواژه
                ops.Password.RequiredLength = 8;
                ops.Password.RequireNonAlphanumeric = false;
                ops.Password.RequireLowercase = false;
                ops.Password.RequireUppercase = false;
                ops.Password.RequireDigit = false;
                //جهت اهراز سطح دسترسی کاربر

                ops.Cookies.ApplicationCookie.AutomaticAuthenticate = true;
                ops.Cookies.ApplicationCookie.AutomaticChallenge = true;
               //مسیر فرم لاگین
                ops.Cookies.ApplicationCookie.LoginPath = "/Account/AccountView";
                // Cookie settings
                ops.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                ops.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOut";

                // Lockout settings
                //ops.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //ops.Lockout.MaxFailedAccessAttempts = 10;

              



            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //   var physicalProvider = _hostingEnvironment.ContentRootFileProvider;
            //var embeddedProvider = new EmbeddedFileProvider(Assembly.GetEntryAssembly());
            //var compositeProvider = new CompositeFileProvider(physicalProvider, embeddedProvider);

            // choose one provider to use for the app and register it
            //services.AddSingleton<IFileProvider>(physicalProvider);
            //services.AddSingleton<IFileProvider>(embeddedProvider);
            //services.AddSingleton<IFileProvider>(compositeProvider);

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddMvc();


            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<IEmailForForgetPassword, EmailForForgetPassword>();
            //services.AddScoped<IShoppingCart, ShoppingCart>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            env.EnvironmentName = EnvironmentName.Development;

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler("/Error/Index");
                app.UseStatusCodePagesWithReExecute("/Error/Index/{0}");
            }

            app.UseStaticFiles();
            //app.UseFileServer();

            app.UseIdentity();









            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
                //routes.MapRoute(
                //   name: "Products",
                //   template: "{controller=Products}/{action=Index}/{id?}");
            });
            //SeedData Create Role Administrator & User And Account UserAdmin
            IdentitySeedData.EnsurePopulated(app.ApplicationServices).Wait();
        }
    }
}
