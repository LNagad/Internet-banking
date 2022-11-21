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
    public static class DefaultBasicUser
    {

        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            ApplicationUser defaultUser = new();
            defaultUser.UserName = "basicUser";
            defaultUser.Email = "BasicUser@internetBanking.com";
            defaultUser.FirstName = "Basic";
            defaultUser.LastName = "User";
            defaultUser.EmailConfirmed = true;
            defaultUser.Cedula = "00000000000";
            defaultUser.Status = true;
            defaultUser.PhoneNumberConfirmed = true;

            if(userManager.Users.All(p => p.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);   

                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                }
            }
        }

    }
}
