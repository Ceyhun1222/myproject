using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ObstacleManagementSystem.Common;
using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.ViewComponents
{
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class SidebarViewComponent : ViewComponent
    {
        private IStringLocalizer<SidebarViewComponent> _localizer;

        public SidebarViewComponent(IStringLocalizer<SidebarViewComponent> stringLocalizer)
        {
            _localizer = stringLocalizer;
        }

        public IViewComponentResult Invoke(string filter)
        {
            //you can do the access rights checking here by using session, user, and/or filter parameter
            var sidebars = new List<SidebarMenu>();

            //if (((ClaimsPrincipal)User).GetUserProperty("AccessProfile").Contains("VES_008, Payroll"))
            //{
            //}
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Home,_localizer));
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Request, _localizer, Tuple.Create(0, 1, 0)));
            sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.Users, _localizer));
            //sidebars.Add(ModuleHelper.AddModule(ModuleHelper.Module.UserRegistration, _localizer, Tuple.Create(0, 1, 1)));
            sidebars.Last().TreeChild = new List<SidebarMenu>()
            {
                ModuleHelper.AddModule(ModuleHelper.Module.AllUsers, _localizer, Tuple.Create(0, 1, 0)),
                ModuleHelper.AddModule(ModuleHelper.Module.UserRegistration, _localizer, Tuple.Create(0, 1, 1))
            };
            return View(sidebars);
        }
    }
}
