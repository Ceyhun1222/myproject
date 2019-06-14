using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Aran.Omega.Models;

namespace Aran.Omega.View
{
    /// <summary>
    /// Interaction logic for AbstactView.xaml
    /// </summary>
    public partial class AbstractView : Window
    {
        private IList<Info> _propList;

        public AbstractView()
        {
            InitializeComponent();
        }

        public void LoadSurfaces(IList<Info> propList,string header )
        {
            _propList = propList;
            grpMain.Header = header;
            this.Title = header;
            Changed();
        }

        void Changed()
        {
            List<UIElement> uies = new List<UIElement>();

            foreach (var prop in _propList)
                uies.Add(makeStringProperty(prop));

            this.Height = 100 + (27*_propList.Count);
            this.Width = 200+(7 * _propList.Max(prop => prop.Name.Length));
  
            StackPanel st = new StackPanel();
            st.Orientation = Orientation.Horizontal;
            st.HorizontalAlignment = HorizontalAlignment.Center;
            st.Margin = new Thickness(0, 20, 0, 0);
            foreach (var uie in uies)
            {
                if (uie is Button)
                    st.Children.Add(uie);
                else
                    container.Children.Add(uie);
            }
            if (st.Children.Count > 0)
                container.Children.Add(st);

        }

        UIElement makeStringProperty(Info prop)
        {
            TextBox txtValue = new TextBox();
            txtValue.FontSize = 12;
            txtValue.VerticalAlignment = VerticalAlignment.Center;
            txtValue.Text = prop.Value;
            txtValue.IsEnabled = false;

            Label lblName = new Label();
            lblName.Content = prop.Name+" : ";
            lblName.FontWeight =FontWeights.Black;
            lblName.Margin = new Thickness(3) ;

            Label lblUnit = new Label();
            lblUnit.Content = prop.Unit;
            lblName.Margin = new Thickness(2,2,2,3);

            Grid u = new Grid();
            u.ColumnDefinitions.Add(new ColumnDefinition());
            u.ColumnDefinitions.Add(new ColumnDefinition());
            u.ColumnDefinitions.Add(new ColumnDefinition{Width= new GridLength(20, GridUnitType.Pixel)});


            Grid.SetColumn(lblName, 0);
            Grid.SetRow(lblName, 0);
            
            Grid.SetColumn(txtValue, 1);
            Grid.SetRow(txtValue, 0);
            
            Grid.SetColumn(lblUnit, 2);
            Grid.SetRow(lblUnit, 0);
            
            u.Children.Add(lblName);
            u.Children.Add(txtValue);
            u.Children.Add(lblUnit);
            return u;
        }
    }
}
