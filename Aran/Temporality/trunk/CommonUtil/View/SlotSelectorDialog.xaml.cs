using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.ViewModel;

namespace Aran.Temporality.CommonUtil.View
{
    /// <summary>
    /// Interaction logic for SlotSelectorDialog.xaml
    /// </summary>
    public partial class SlotSelectorDialog : Window
    {
        public SlotSelectorDialog()
        {
            InitializeComponent();

            Loaded += (a, b) =>
            {
                var model=DataContext as SlotSelectorViewModel;
                if (model == null) return;
                model.LoadPublicSlots();
                model.OnClose = () =>
                {
                    try
                    {
                        if (Application.Current == null) Close();
                        else
                            Application.Current.Dispatcher.Invoke(
                                DispatcherPriority.Normal,
                                (Action)(Close));
                    }
                    catch (Exception ex)
                    {
                        LogManager.GetLogger(typeof(SlotSelectorDialog)).Error(ex);
                    }
                };
            };
        }
    }
}
