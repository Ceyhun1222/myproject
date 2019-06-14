using Aerodrome.Features;
using Framework.Stasy.Context;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfEnvelope.Crud.UserControls
{
    /// <summary>
    /// Interaction logic for RelatedFeatureControl.xaml
    /// </summary>
    public partial class RelatedFeatureControl : UserControl
    {
        public RelatedFeatureControl()
        {
            InitializeComponent();
           
        }



        public List<Type> FeatureTypes
        {
            get { return (List<Type>)GetValue(FeatureTypesProperty); }
            set { SetValue(FeatureTypesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FeatureTypes.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FeatureTypesProperty =
            DependencyProperty.Register("FeatureTypes", typeof(List<Type>), typeof(RelatedFeatureControl), new FrameworkPropertyMetadata(null, null));


        public AM_AbstractFeature SelectedFeature
        {
            get { return (AM_AbstractFeature)GetValue(SelectedFeatureProperty); }
            set { SetValue(SelectedFeatureProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedFeature.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedFeatureProperty =
            DependencyProperty.Register("SelectedFeature", typeof(AM_AbstractFeature), typeof(RelatedFeatureControl), new FrameworkPropertyMetadata(null,
        new PropertyChangedCallback(RelatedFeatureControl.OnSourceChanged)));


        public SelectionChangedEventHandler ValueChanged;

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            RelatedFeatureControl control = (RelatedFeatureControl)d;

            control.ValueChanged?.Invoke(null, null);
        }

        //private void typesCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if(SelectedFeature.GetType().Name.Equals(typesCombobox.Text))
        //    {

        //    }
        //    var selectedType = typesCombobox.SelectedItem as Type;
        //    var featuresByType = AerodromeDataCash.ProjectEnvironment.Context.FeatureCollections[selectedType];
        //    featuresCombobox.ItemsSource = featuresByType;
        //}
    }
}
