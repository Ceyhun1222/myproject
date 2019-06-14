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
using System.ComponentModel;
using System.Windows.Media.Animation;

namespace Accent.Loader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bgWorker;
        public bool IsWorking = true;
        public MainWindow()
        {
            InitializeComponent();
            Duration duration = new Duration(TimeSpan.FromSeconds(1));

            DoubleAnimation doubleAnimation = new DoubleAnimation(100, duration);

            doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            pgrBar.BeginAnimation(ProgressBar.ValueProperty, doubleAnimation);

            bgWorker = new BackgroundWorker();

            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);

            bgWorker.ProgressChanged += new ProgressChangedEventHandler(bgWorker_ProgressChanged);

            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);
            bgWorker.RunWorkerAsync();
        }

        void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
           // this.Close();
        }

        void bgWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
        }

        void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (IsWorking)
            {
                Console.WriteLine("Still Working...");
            }
        }


    }
}
