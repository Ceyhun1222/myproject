using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.Data
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configuration)
        {
            SeedRoles(roleManager);
            SeedUsersAsync(userManager,configuration);
        }

        public static void SeedUsersAsync
(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            var username = configuration["AdminUserName"];
            var user = userManager.FindByNameAsync
                (username).Result;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Firstname = username,
                    Lastname = username,
                    DateRegistered = DateTime.Now,
                    Birthday = DateTime.Now,
                    Gender = Gender.Male,
                };

                IdentityResult result = userManager.CreateAsync
                    (user, configuration["AdminParol"]).Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Roles.Admin.ToString()).Wait();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        throw new Exception(error.Description);
                    }
                }
            }           
        }

        public static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            foreach (var role in Enum.GetValues(typeof(Roles)))
            {
                var exist = roleManager.RoleExistsAsync(role.ToString()).Result;
                if (!exist)
                {
                    var roleNormal = new ApplicationRole
                    {
                        Name = role.ToString()
                    };
                    var roleResult = roleManager.CreateAsync(roleNormal).Result;
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            throw new Exception(error.Description);
                        }                        
                    }
                }
            }
        }
    }
}
