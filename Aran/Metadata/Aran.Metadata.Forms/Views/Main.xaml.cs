using Aran.Aim.Features;
using MahApps.Metro.Controls;
using Aran.Metadata.Forms.ViewModels;
using System.Collections.Generic;
using System;
using Aran.Aim.Metadata.ISO;
using System.Linq;
using System.Windows;
using Aran.Aim.Data;
using Aran.Temporality.Common.Logging;

namespace Aran.Metadata.Forms.Views
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : MetroWindow
    {
        private readonly MetadataViewModel _metadataViewModel;

        public Main(List<MdMetadata> metadataList, List<User> originators)
        {
            InitializeComponent();

            _metadataViewModel = new MetadataViewModel(metadataList, originators);

            try
            {
                _metadataViewModel.Init();
            }
            catch (Exception e)
            {
                LogManager.GetLogger("Aran.Metadata.Forms.Views.Main").Error(e, e.Message);
                throw;
            }
            _metadataViewModel.RequestClose += Close;
            this.DataContext = _metadataViewModel;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _metadataViewModel.Clear();
        }
    }
}
