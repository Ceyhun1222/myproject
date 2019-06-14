using System;
using Microsoft.Extensions.Localization;
using ObstacleManagementSystem.Models;
using ObstacleManagementSystem.ViewComponents;

namespace ObstacleManagementSystem.Common
{
    /// <summary>
    /// This is where you customize the navigation sidebar
    /// </summary>
    public static class ModuleHelper
    {
        public enum Module
        {
            Home,
            Users,
            UserRegistration,
            Request,
            AllUsers,
            //Error,
            //Login,
            //Register,
        }

        public static SidebarMenu AddHeader(string name)
        {
            return new SidebarMenu
            {
                Type = SidebarMenuType.Header,
                Name = name,
            };
        }

        public static SidebarMenu AddModule(Module module, IStringLocalizer<SidebarViewComponent> localizer, Tuple<int, int, int> counter = null)
        {
            if (counter == null)
                counter = Tuple.Create(0, 0, 0);

            switch (module)
            {
                case Module.Home:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = localizer["Home"],
                        IconClassName = "fa fa-link",
                        Area = "",
                        Controller = "Home",
                        Action = "Index",
                        LinkCounter = counter,
                    };
                case Module.Request:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = localizer["Requests"],
                        IconClassName = "fa fa-file",
                        Area = "",
                        Controller = "Request",
                        Action = "Index",
                        LinkCounter = counter,
                    };
                case Module.Users:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Tree,
                        IsActive = false,
                        Name = localizer["Users"],
                        IconClassName = "fa fa-users",
                        Area = "",
                        Controller = "User",
                        Action = "Index",
                    };
                case Module.AllUsers:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Tree,
                        IsActive = false,
                        Name = localizer["All Users"],
                        IconClassName = "fa fa-group",
                        Area = "",
                        Controller = "User",
                        Action = "Index",
                    };
                case Module.UserRegistration:
                    return new SidebarMenu
                    {
                        Type = SidebarMenuType.Link,
                        Name = localizer["User registration"],
                        IconClassName = "fa fa-user-plus",
                        Area = "",
                        Controller = "Account",
                        Action = "Register",
                        LinkCounter = counter,
                    };
                default:
                    return null;
            }            
        }
    }
}
