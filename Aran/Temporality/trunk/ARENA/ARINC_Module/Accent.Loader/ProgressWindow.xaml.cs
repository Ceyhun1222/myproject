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
using System.Threading;
using System.Windows.Media.Animation;

namespace Process.Loader
{
    /// <summary>
    /// Interaction logic for ProgressWindow.xaml
    /// </summary>
    public partial class ProgressWindow : Window
    {

        public ProgressWindow()
        {
           
            InitializeComponent();
           
        }

        public ProgressWindow(MockData mdata)
            : this()
        {
            mdata.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(mdata_PropertyChanged);
        }

        void mdata_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            String value = (sender as MockData).Text;
            int percent = (sender as MockData).Percentage;

            if ( loadingStatus.Dispatcher.Thread != Thread.CurrentThread )
            {
                loadingProgress.Dispatcher.Invoke(new Action(() => loadingProgress.Value = percent));
                loadingStatus.Dispatcher.Invoke(new Action(() => loadingStatus.Content = value));
            }
            else
            {
                loadingProgress.Value = (sender as MockData).Percentage;
                loadingStatus.Content = (sender as MockData).Text;
            }

            if ( percent == 100 )
            {
                if ( this.Dispatcher.Thread != Thread.CurrentThread )
                    this.Dispatcher.Invoke(new Action(() => this.Hide()));
                else
                    this.Hide();

            }
        }

        public void ShowWindow()
        {
            if ( this.Dispatcher.Thread == Thread.CurrentThread )
            {
                this.Show();
            }
            else
            {
                Action act = () =>
                    {
                        this.Show();
                    };
                this.Dispatcher.Invoke(act);
            }
        }

        public void SetProgressStyle(bool intermadiate)
        {
            if ( loadingProgress.Dispatcher.Thread != Thread.CurrentThread )
            {
                loadingProgress.Dispatcher.Invoke(new Action(() => loadingProgress.IsIndeterminate = intermadiate));
            }
            else
                loadingProgress.IsIndeterminate = intermadiate;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
