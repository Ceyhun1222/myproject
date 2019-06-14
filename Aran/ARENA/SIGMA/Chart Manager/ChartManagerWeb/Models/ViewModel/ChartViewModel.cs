using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ChartManagerWeb.ChartServiceReference;
using ChartManagerWeb.Helper;

namespace ChartManagerWeb.Models.ViewModel
{
    public class ChartViewModel
    {
        public List<Chart4View> ChartList { get; set; }

        [Display(Name = "Select user")]
        public List<SelectListItem> UserList {get;set;}
        public long SelectedUser { get; set; }


        [Display(Name="Select type")]
        public ChartType SelectedChartType { get; set; }

        [Display(Name = "Select airport")]
        public List<SelectListItem> AerodromeList { get; set; }
        public string SelectedAerodrome { get; set; }

        [Display(Name = "Select runway direction")]
        public List<SelectListItem> RunwayDirList { get; set; }
        public string SelectedRwyDir { get; set; }

        [Display(Name = "Select status (locked)")]
        public List<SelectListItem> StatusList { get; set; }
        public string SelectedStatus { get; set; }

        [Display(Name = "Begin creation")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedTimeFrom { get; set; }

        [Display(Name = "End creation")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedTimeTo { get; set; }

        [Display(Name = "Begin effective")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveFrom { get; set; }

        [Display(Name = "End effective")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EffectiveTo { get; set; }

        [DefaultValue("Search")]
        public string Name { get; set; }
    }
}