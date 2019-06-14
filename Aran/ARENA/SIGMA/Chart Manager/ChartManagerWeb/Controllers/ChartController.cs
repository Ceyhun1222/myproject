using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ChartManagerWeb.ChartServiceReference;
using ChartManagerWeb.Helper;
using ChartManagerWeb.Helper.Attribute;
using ChartManagerWeb.Models.ViewModel;

namespace ChartManagerWeb.Controllers
{
    [LogActionFilter]
    [SessionExpire]
    public class ChartController : BaseController
    {
        [HttpGet]
        //[AuthorizeUser]
        public ActionResult Index()
        {
            //CheckSession();
            //if (!IsAuthenticated)
            //   return RedirectToAction("Login", "Account");
            var chartViewModel = PopulateChartViewModel();
            chartViewModel.CreatedTimeFrom = DateTime.Now.AddDays(-90);
            chartViewModel.CreatedTimeTo = DateTime.Now.AddDays(90);
            chartViewModel.EffectiveFrom = DateTime.Now;
            chartViewModel.EffectiveTo = DateTime.Now.AddDays(150);
            ServiceClient.GetAllCharts(ChartType.All, string.Empty, null, null, null, null, null,
                    chartViewModel.CreatedTimeFrom, chartViewModel.CreatedTimeTo,
                    chartViewModel.EffectiveFrom, chartViewModel.EffectiveTo)
                .ForEach(t => chartViewModel.ChartList.Add(t.ConvertTo()));

            return View(chartViewModel);
        }

        private ChartViewModel PopulateChartViewModel()
        {
            ChartViewModel result = new ChartViewModel()
            {
                UserList = new List<SelectListItem>()
                {
                    new SelectListItem {Text = "All", Value = "0"}
                },
                AerodromeList = new List<SelectListItem>()
                {
                    new SelectListItem {Text = "All", Value = "0"}
                },
                RunwayDirList = new List<SelectListItem>()
                {
                    new SelectListItem {Text = "All", Value = "0"}
                },
                StatusList = new List<SelectListItem>(),
                ChartList = new List<Models.Chart4View>()
            };

            FilterBuilder.CreateFilterItems().ForEach(t =>
                result.StatusList.Add(new SelectListItem() {Text = t.ToString()}));
            ServiceClient.GetAllUser().ForEach(t =>
                result.UserList.Add(new SelectListItem()
                {
                    Text = t.FirstName + " " + t.LastName,
                    Value = t.Id.ToString()
                }));
            ServiceClient.GetAerodromes(string.Empty).ForEach(t =>
                result.AerodromeList.Add(new SelectListItem {Text = t}));
            if (result.AerodromeList.Count > 1)
                ServiceClient.GetRunways(result.AerodromeList[1].Text).ForEach(t =>
                    result.RunwayDirList.Add(new SelectListItem {Text = t}));

            return result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ChartViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ChartViewModel viewModelDefault = PopulateChartViewModel();
                    viewModel.AerodromeList = viewModelDefault.AerodromeList;
                    viewModel.RunwayDirList = viewModelDefault.RunwayDirList;
                    viewModel.StatusList = viewModelDefault.StatusList;
                    viewModel.UserList = viewModelDefault.UserList;
                    viewModel.ChartList = viewModelDefault.ChartList;
                    long? userId = null;
                    if (viewModel.SelectedUser != 0)
                        userId = viewModel.SelectedUser;
                    bool? status = null;
                    if (viewModel.SelectedStatus != "All")
                        status = viewModel.SelectedStatus.ToLower() == "yes";
                    var lst = ServiceClient.GetAllCharts(viewModel.SelectedChartType, viewModel.Name, userId,
                            string.Empty,(viewModel.SelectedAerodrome != "0") ? viewModel.SelectedAerodrome : string.Empty,
                            string.Empty, status, viewModel.CreatedTimeFrom, viewModel.CreatedTimeTo,
                            viewModel.EffectiveFrom, viewModel.EffectiveTo);
                    foreach (var item in lst)
                    {
                        viewModel.ChartList.Add(item.ConvertTo());
                    }
                    return View(viewModel);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex);
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        //[AuthorizeUser]
        public ActionResult Details(long id)
        {
            //CheckSession();
            if (!Authenticated)
                return RedirectToAction("Login", "Account");
            return View(ServiceClient.GetChartById(id).ConvertTo());
        }

        [HttpGet]
        public JsonResult GetImage(long id)
        {
            var prevByteArray = Convert.ToBase64String(ServiceClient.GetPreviewOf(id));
            var data = new {ok = true, preview = prevByteArray};
            var json = new JsonResult()
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };

            //}data, JsonRequestBehavior.AllowGet);
            //json.MaxJsonLength = int.MaxValue;
            //json.ContentType = 
            return json;
        }

        [HttpGet]
        //[AuthorizeUser]
        public ActionResult Delete(long id)
        {
            //CheckSession();
            if (!Authenticated)
                return RedirectToAction("Login", "Account");
            return View(ServiceClient.GetChartById(id));
        }

        [HttpPost]
        //[AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id, FormCollection collection)
        {
            //CheckSession();
            if (!Authenticated)
                return RedirectToAction("Login", "Account");
            try
            {
                // TODO: Add delete logic here
                ServiceClient.DeleteChartById(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
