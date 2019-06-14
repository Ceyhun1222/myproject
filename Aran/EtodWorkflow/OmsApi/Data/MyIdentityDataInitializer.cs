using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using OmsApi.Entity;
using OmsApi.Configuration;

namespace OmsApi.Data
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, 
            IConfiguration configuration, AdminConfig adminSettings, ApplicationDbContext dbContext)
        {
            SeedRoles(roleManager);
            SeedUsersAsync(userManager,configuration,adminSettings);
            //SeedSlot(dbContext);
        }

        //private static void SeedSlot(ApplicationDbContext dbContext)
        //{
        //    if(dbContext.SelectedSlot == null)
        //    {
        //        try
        //        {
        //            dbContext.SelectedSlot = new Slot() { SlotId = 0 };
        //        }
        //        catch (Exception ex)
        //        {
        //            var k = ex.Message;
        //            throw;
        //        }
                
        //    }
        //}

        public static void SeedUsersAsync
(UserManager<ApplicationUser> userManager, IConfiguration configuration, AdminConfig adminSettings)
        {
            var username = adminSettings.UserName;
            var user = userManager.FindByNameAsync
                (username).Result;
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = username,
                    Firstname = username,
                    Lastname = username,
                    CreatedAt = DateTime.Now,
                    LastModifiedAt = DateTime.Now,
                    Status = Status.Accepted,
                    Company = new Company()
                    //Airport = new AirportHeliport(),
                };

                IdentityResult result = userManager.CreateAsync
                    (user, adminSettings.Password).Result;

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