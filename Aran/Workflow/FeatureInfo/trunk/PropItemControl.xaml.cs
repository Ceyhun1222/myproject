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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Aran.Aim.FeatureInfo;
using System.Globalization;

namespace Aran.Aim.FeatureInfo
{
    internal partial class PropItemControl : UserControl
    {
        public event EventHandler ComplexPropClicked;
        public event OpenFeatureEventHandler ReferenceOpened;

        public PropItemControl ()
        {
            InitializeComponent ();

            Loaded += new RoutedEventHandler (PropItemControl_Loaded);
        }


        private void PropItemControl_Loaded (object sender, RoutedEventArgs e)
        {
            var dc = DataContext;
            BindingInfo bindInfo = dc as BindingInfo;
            var valueControl = CreateValueControl (bindInfo);
            if (valueControl == null)
                return;
            ui_valueContainerGrid.Children.Add (valueControl);

            var tb = ui_propTB;

            var formattedText = new FormattedText (
                tb.Text, CultureInfo.CurrentCulture, tb.FlowDirection,
                new Typeface (
                    tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch),
                tb.FontSize, Brushes.Black);

            bool isToolVisible = (formattedText.Width > tb.Width - 10);

            if (isToolVisible)
            {
                tb = new TextBlock ();
                tb.Text = bindInfo.Name;
                ui_propTB.ToolTip = tb;
            }

            //if (!isToolVisible)
            //    ui_propTB.ToolTip = null;
        }

        private FrameworkElement CreateValueControl (BindingInfo bindInfo)
        {
            switch (bindInfo.InfoType)
            {
                case BindingInfoType.Primitive:
                    return new PrimitiveValueControl ();
                case BindingInfoType.ValClass:
                    return new ValClassValueControl ();
                case BindingInfoType.Complex:
                    {
                        var cont = new ComplexValueControl ();
                        cont.OpenAimPropClicked += new EventHandler (ComplexValueControl_OpenAimPropClicked);
                        return cont;
                    }
                case BindingInfoType.Reference:
                    {
                        var cont = new ReferenceValueControl ();
                        cont.FeatureOpened += new OpenFeatureEventHandler (ReferenceValueControl_FeatureOpened);
                        return cont;
                    }
            }
            return null;
        }

        private void ReferenceValueControl_FeatureOpened (object sender, OpenFeatureEventArgs e)
        {
            if (ReferenceOpened == null)
                return;

            ReferenceOpened (sender, e);
        }

        private void ComplexValueControl_OpenAimPropClicked (object sender, EventArgs e)
        {
            if (ComplexPropClicked == null)
                return;

            ComplexPropClicked (sender, e);
        }
    }
}
