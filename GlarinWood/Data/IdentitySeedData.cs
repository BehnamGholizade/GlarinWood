using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GlarinWood.Models;

namespace GlarinWood.Data
{
    //SEED
    public static class IdentitySeedData
    {
        //نام کاربری و گذرواژه
        private const string adminUserEmail = "admin@info.com";
        private const string adminPassword = "admin12345";
        //مقدار دهی متغییرهای نقش کاربر، جهت ساخت نقش ها و انتساب آن به ادمین
        private const string adminRole = "Administrator"; 
        private const string userRole = "User";

        public static async Task EnsurePopulated(IServiceProvider ApplicationServices)

        { 
            UserManager<ApplicationUser> _userManager = ApplicationServices
              .GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> _roleManager = ApplicationServices
            .GetRequiredService<RoleManager<IdentityRole>>();

            if (await _userManager.FindByEmailAsync(adminUserEmail) == null)
            {
                {
                     //ساخت نقش ادمین

                    if (await _roleManager.FindByNameAsync(adminRole) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(adminRole));
                    }
                    //ساخت نقش کاربر
                    if (await _roleManager.FindByNameAsync(userRole) == null)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(userRole));
                    }

                    var user = new ApplicationUser { UserName = adminUserEmail, Email = adminUserEmail };
                    IdentityResult result = await _userManager.CreateAsync(user, adminPassword);
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, adminRole);
                        await _userManager.AddToRoleAsync(user, userRole);

                    } 
                }
            }

            //context.Database.EnsureCreated();

            // Look for any Users.
            //if (context.Users.Any())
            //{
            //    return;   // DB has been seeded
            //}

        }
    }
}
