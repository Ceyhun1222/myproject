using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ObstacleManagementSystem.Common.Attributes;
using ObstacleManagementSystem.Models;

namespace ObstacleManagementSystem.Controllers
{
    [Authorize(Roles = nameof(Roles.Admin) + "," + nameof(Roles.Moderator))]
    [MiddlewareFilter(typeof(LocalizationPipeline))]
    public class HomeController : BaseController
    {
        private readonly IStringLocalizer<HomeController> _localizer;

        public HomeController(IStringLocalizer<HomeController> stringLocalizer)
        {
            _localizer = stringLocalizer;
        }

        [HelpDefinition]
        public IActionResult Index()
        {
            AddPageHeader(_localizer["Main panel"], "");
            return View();
        }

        [HttpPost]
        public IActionResult Index(object model)
        {
            AddPageAlerts(PageAlertType.Info, _localizer["you may view the summary <a href='#'>here</a>"]);
            return View("Index");
        }

        [HelpDefinition]
        public IActionResult About()
        {
            ViewData["Message"] = _localizer["Your application description page."];
            AddBreadcrumb(_localizer["About"], "/Account/About");
            return View();
        }

        [HelpDefinition("helpdefault")]
        public IActionResult Contact()
        {
            AddBreadcrumb(_localizer["Register"], "/Account/Register");
            AddBreadcrumb(_localizer["Contact"], "/Account/Contact");
            ViewData["Message"] = _localizer["Your contact page"];
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        #region Helpers

        #region Sort by column with order method

        /// <summary>
        /// Sort by column with order method.
        /// </summary>
        /// <param name="order">Order parameter</param>
        /// <param name="orderDir">Order direction parameter</param>
        /// <param name="data">Data parameter</param>
        /// <returns>Returns - Data</returns>
        private List<SalesOrderDetail> SortByColumnWithOrder(string order, string orderDir, List<SalesOrderDetail> data)
        {
            // Initialization.
            List<SalesOrderDetail> lst = new List<SalesOrderDetail>();

            try
            {
                // Sorting
                switch (order)
                {
                    case "0":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.sr).ToList()
                                                                                                 : data.OrderBy(p => p.sr).ToList();
                        break;

                    case "1":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ordertracknumber).ToList()
                                                                                                 : data.OrderBy(p => p.ordertracknumber).ToList();
                        break;

                    case "2":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.quantity).ToList()
                                                                                                 : data.OrderBy(p => p.quantity).ToList();
                        break;

                    case "3":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.productname).ToList()
                                                                                                 : data.OrderBy(p => p.productname).ToList();
                        break;

                    case "4":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.specialoffer).ToList()
                                                                                                   : data.OrderBy(p => p.specialoffer).ToList();
                        break;

                    case "5":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.unitprice).ToList()
                                                                                                 : data.OrderBy(p => p.unitprice).ToList();
                        break;

                    case "6":
                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.unitpricediscount).ToList()
                                                                                                 : data.OrderBy(p => p.unitpricediscount).ToList();
                        break;

                    default:

                        // Setting.
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.sr).ToList()
                                                                                                 : data.OrderBy(p => p.sr).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.
                Console.Write(ex);
            }

            // info.
            return lst;
        }

        #endregion

        #endregion
    }
}
