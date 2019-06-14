using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms.Integration;

namespace Aran.Delta.View
{
    public class WinFormsWrapper : WindowsFormsHost
    {
        //You'll have to setup the control as needed
        //public WinFormsWrapper()
        //{
        //    this.Child = new ChoosePointNS.CoordinateControl();
        //    var wrapper = this.Child as ChoosePointNS.CoordinateControl;
        //    wrapper.LatitudeChanged  += Latitude_Changed;
        //    wrapper.LongtitudeChanged += Longtitude_Changed;
        //}

        //void Latitude_Changed(object sender, EventArgs e)
        //{
        //    if (LatitudeChangeEvent != null)
        //        LatitudeChangeEvent(sender, null);
        //}

        //void Longtitude_Changed(object sender, EventArgs e)
        //{
        //    if (LongtitudeChangeEvent != null)
        //        LongtitudeChangeEvent(sender, null);
        //}

        //public static readonly DependencyProperty IsDDProperty = DependencyProperty.Register("IsDD", typeof(bool), typeof(WinFormsWrapper),
        //     new UIPropertyMetadata(true, new PropertyChangedCallback(IsDD_Changed)));

        //public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.Register("IsEnabled", typeof(bool), typeof(WinFormsWrapper),
        //     new UIPropertyMetadata(true, new PropertyChangedCallback(IsEnabled_Changed)));

        //public static readonly DependencyProperty LatitudeProperty = DependencyProperty.Register("Latitude", typeof(double), typeof(WinFormsWrapper),
        //     new UIPropertyMetadata(new Double(), new PropertyChangedCallback(Latitude_Changed)));

        //public static readonly DependencyProperty LongtitudeProperty = DependencyProperty.Register("Longtitude", typeof(double), typeof(WinFormsWrapper),
        //     new UIPropertyMetadata(new Double(), new PropertyChangedCallback(Longtitude_Changed)));

        //public static readonly DependencyProperty AccuracyProperty = DependencyProperty.Register("Accuracy", typeof(double), typeof(WinFormsWrapper),
        //     new UIPropertyMetadata(new Double(), new PropertyChangedCallback(Accuracy_Changed)));

        
        //private static void Accuracy_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var wrapper = sender as WinFormsWrapper;
        //    (wrapper.Child as ChoosePointNS.CoordinateControl).Accuracy = Convert.ToInt32(e.NewValue);
        //}

        //private static void IsEnabled_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var wrapper = sender as WinFormsWrapper;
        //    (wrapper.Child as ChoosePointNS.CoordinateControl).Enabled = (bool)e.NewValue;
        //}

        //private static void IsDD_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var wrapper = sender as WinFormsWrapper;
        //    (wrapper.Child as ChoosePointNS.CoordinateControl).IsDD = (bool)e.NewValue;
        //}

        //private static void Latitude_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var wrapper = sender as WinFormsWrapper;
        //    (wrapper.Child as ChoosePointNS.CoordinateControl).Latitude = (double)e.NewValue;
        //}

        //private static void Longtitude_Changed(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        //{
        //    var wrapper = sender as WinFormsWrapper;
        //    (wrapper.Child as ChoosePointNS.CoordinateControl).Longtitude = (double)e.NewValue;
        //}

        //public bool IsEnabled
        //{
        //    get { return (bool)GetValue(IsEnabledProperty); }
        //    set { SetValue(IsEnabledProperty, value); }
        //}

        //public bool IsDD
        //{
        //    get { return (bool)GetValue(IsDDProperty); }
        //    set { SetValue(IsDDProperty, value); }
        //}


        //public int Accuracy
        //{
        //    get { return (int)GetValue(AccuracyProperty); }
        //    set { SetValue(AccuracyProperty, value); }
        //}
        

        //public double Latitude
        //{
        //    get { return (double)GetValue(LatitudeProperty); }
        //    set { SetValue(LatitudeProperty, value); }
        //}

        //public double Longtitude
        //{
        //    get { return (double)GetValue(LongtitudeProperty); }
        //    set { SetValue(LongtitudeProperty, value); }
        //}

        //public string Text { get; set; }

        //public EventHandler LatitudeChangeEvent {get;set;}
        //public EventHandler LongtitudeChangeEvent { get; set; }

        
    }
}
