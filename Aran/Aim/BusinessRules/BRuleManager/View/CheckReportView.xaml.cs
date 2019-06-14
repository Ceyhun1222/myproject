using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BRuleManager.View
{
    /// <summary>
    /// Interaction logic for CheckReportView.xaml
    /// </summary>
    public partial class CheckReportView : UserControl
    {
        public CheckReportView()
        {
            InitializeComponent();

            Loaded += (s, e) =>
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ui_listView.ItemsSource);
                PropertyGroupDescription groupDescription = new PropertyGroupDescription("BRuleAndProfile");
                view.GroupDescriptions.Add(groupDescription);
            };

            DataContextChanged += (s, e) =>
            {
                if (e.NewValue != null && TryFindResource("r_reportToRule") is ReportToRuleConverter conv)
                    conv.Model = e.NewValue;

                (e.NewValue as dynamic).GetListSelectedIndex = new Func<int>(() => ui_listView.SelectedIndex);
            };
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                (DataContext as dynamic).SearchText = (sender as TextBox).Text;
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            var expander = FindParent<Expander>(sender as DependencyObject);
            if (expander != null)
                expander.IsExpanded = true;
        }

        private static T FindParent<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(dependencyObject);

            if (parent == null)
                return null;

            var parentT = parent as T;
            return parentT ?? FindParent<T>(parent);
        }

        public void FindChilds(DependencyObject o, Type childType, List<DependencyObject> childs)
        {
            if (o != null)
            {
                int childrenCount = VisualTreeHelper.GetChildrenCount(o);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(o, i);
                    if (child.GetType() != childType)
                    {
                        FindChilds(child, childType, childs);
                    }
                    else
                    {
                        childs.Add(child);
                    }
                }
            }
        }

        private void SaveReport_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save Report",
                Filter = "HTML Files (*.html)|*.html|All Files (*.*)|*.*",
                FileName = "brule-report"
            };

            if (dialog.ShowDialog() == true)
                (DataContext as dynamic).SaveReport(dialog.FileName);
        }

        private void CollapseAll_Click(object sender, RoutedEventArgs e)
        {
            CollapseExpandeGroups(false);
        }

        private void ExpandAll_Click(object sender, RoutedEventArgs e)
        {
            CollapseExpandeGroups(true);
        }

        private void CollapseExpandeGroups(bool isExpanded)
        {
            var childs = new List<DependencyObject>();
            FindChilds(ui_listView, typeof(Expander), childs);

            foreach (var item in childs)
            {
                if (item is Expander eo)
                    eo.IsExpanded = isExpanded;
            }
        }

        private void CopyIdentifier_Click(object sender, RoutedEventArgs e)
        {
            var sb = new StringBuilder();
            foreach (var item in ui_listView.SelectedItems)
            {
                if (item is ExpandoObject eo)
                {
                    if (sb.Length > 0)
                        sb.Append(", ");
                    sb.Append((eo as dynamic).Identifier);
                }
            }
            if (sb.Length > 0)
                Clipboard.SetText(sb.ToString());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var s = (sender as dynamic).Parent.Parent.PlacementTarget.DataContext.Items[0].BRule;
                Clipboard.SetText(s);
            }
            catch { }
        }
    }


    public class ReportToRuleConverter : IValueConverter
    {
        public object Model { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ExpandoObject eo && Model != null)
            {
                var brule = (Model as dynamic).GetRuleInfo((value as dynamic).BRule);
                return brule;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
