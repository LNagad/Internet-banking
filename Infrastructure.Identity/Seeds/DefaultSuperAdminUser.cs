using Core.Application.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdminUser
    {

        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "superAdminUser";
            defaultUser.Email = "superAdminUser@internetBanking.com";
            defaultUser.FirstName = "SuperAdmin";
            defaultUser.LastName = "User";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;

            if(userManager.Users.All(p => p.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);   

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }

    }
}
