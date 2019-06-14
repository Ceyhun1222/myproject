using System.Windows.Controls;
using ESRI.ArcGIS.DisplayUI;
using ESRI.ArcGIS.Display;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using Aran.AranEnvironment.Symbols;
using System.Drawing;
using Xceed.Wpf.Toolkit;
using Aran.PANDA.Common;
using System.Collections.Generic;
using Aran.Aim.Data;
using Aran.Aim;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Aim.Data.Filters;
using System;
using System.Windows.Media;

namespace Aran.Omega.SettingsUI
{
    /// <summary>
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    /// 
    public enum MenuType
    { 
        Surface,
        Query,
        Interface
    }

    public partial class SettingsControl : UserControl
    {
        private OLSViewModel _olsViewModel;

        public List<AirportHeliport> AdhpList { get; set; }
        public List<OrganisationAuthority> OrgList { get; set; }

        public SettingsControl()
        {
            InitializeComponent();
        }

        public void LoadAll()
        {
            _olsViewModel = new OLSViewModel();
            _olsViewModel.Load();
            this.DataContext = _olsViewModel;
           // TextOptions.SetTextFormattingMode(this, TextFormattingMode.Display); 
        }

    }
}
