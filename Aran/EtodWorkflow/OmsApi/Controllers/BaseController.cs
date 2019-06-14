using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OmsApi.Entity;

namespace OmsApi.Controllers
{
    public class BaseController : ControllerBase
    {
        internal void GetModelStateFor(IdentityResult identityResult)
        {
            foreach (var item in identityResult.Errors)
            {
                ModelState.AddModelError(item.Code, item.Description);
            }
        }

        internal async Task<ApplicationUser> GetUser(UserManager<ApplicationUser> userManager)
        {
            var username = User.Identity.Name;
            //if (string.IsNullOrEmpty(username))
            //    username = "admin";
            return await userManager.FindByNameAsync(username);
        }
    }
}