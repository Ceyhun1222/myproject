//using Aerodrome.DataCash;
using Aerodrome.Features;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using Framework.Stasy.Context;
using Framework.Stasy.Helper;
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
using WpfEnvelope.Crud.Framework;

namespace WpfEnvelope.Crud.UserControls
{
    /// <summary>
    /// Interaction logic for GeometryControl.xaml
    /// </summary>
    public partial class GeometryControl : UserControl
    {
        public GeometryControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(IGeometry), typeof(GeometryControl), new FrameworkPropertyMetadata(null,
       new PropertyChangedCallback(GeometryControl.OnSourceChanged)));

        public static readonly DependencyProperty EditedItemProperty =
             DependencyProperty.Register("EditedItem", typeof(AM_AbstractFeature), typeof(GeometryControl), new FrameworkPropertyMetadata(null, null));

        public IGeometry Value
        {
            get { return (IGeometry)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);               
            }
        }

        public AM_AbstractFeature EditedItem
        {
            get { return (AM_AbstractFeature)GetValue(EditedItemProperty); }
            set
            {
                SetValue(EditedItemProperty, value);
            }
        }

        public SelectionChangedEventHandler ValueChanged;

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            GeometryControl control = (GeometryControl)d;

            control.ValueChanged?.Invoke(null, null);
        }

        private void setGeo_Click(object sender, RoutedEventArgs e)
        {

            IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
            IMap pMap = pMxDoc.FocusMap;
            IEnumFeature pEnumFeat = pMap.FeatureSelection as IEnumFeature;

            int count = 0;
            while (pEnumFeat.Next() != null)
                count++;

            if (count == 0)
            {
                MessageBox.Show("No Feature Selected", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                //    System.Runtime.InteropServices.Marshal.ReleaseComObject(pEnumFeat);
                return;
            }

            if (count > 1)
            {
                MessageBox.Show("More than one feature selected", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            pEnumFeat.Reset();
            IFeature pFeat = pEnumFeat.Next();

            IFields pFields;
            //Retrieving alla Fields value
            IEnumFeatureSetup enumFeatSetup = (IEnumFeatureSetup)pEnumFeat;
            enumFeatSetup.AllFields = true;
            //while (pFeat != null)
            //{


            var featLayer = EsriUtils.getLayerByName(pMap, pFeat.Class.AliasName);
            IFeatureClass selectedFeatClass = ((IFeatureLayer)featLayer).FeatureClass;

            IGeoDataset geoDataset = (IGeoDataset)selectedFeatClass;
            ESRI.ArcGIS.Geometry.ISpatialReference spatialReference = geoDataset.SpatialReference;

            if (spatialReference?.Name is null)
            {
                MessageBox.Show("SpatialReference is null", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (spatialReference.Name != "GCS_WGS_1984")
            {
                MessageBox.Show("Coordinate system must be: WGS 1984", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }


            pFields = pFeat.Fields;
            int indx = pFields.FindField("SHAPE");
            if (indx == -1)
            {
                MessageBox.Show("Shape field not found in selected feature", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                pFeat = pEnumFeat.Next();
                return;
            }
            object value = pFeat.get_Value(indx);
            var oldValue = Value;
            if (value != null)
            {
                //Do something
                try
                {
                    Value = (IGeometry)value;
                }
                catch (Exception ex)
                {
                    Value = oldValue;
                    //hasValueCheckbox.IsChecked = false;
                    MessageBox.Show("Geometry types does not match", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
            }
            //pFeat = pEnumFeat.Next();
            //}


            hasValueCheckbox.IsChecked = true;
            ValueChanged?.Invoke(null, null);
            MessageBox.Show("Geometry assigned", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void clearGeo_Click(object sender, RoutedEventArgs e)
        {
            Value = null;
            hasValueCheckbox.IsChecked = false;
            ValueChanged?.Invoke(null, null);
        }

        private void zoomTo_Click(object sender, RoutedEventArgs e)
        {
            IMxDocument pMxDoc = AerodromeDataCash.ProjectEnvironment.mxApplication.Document as IMxDocument;
            IMap pMap = pMxDoc.FocusMap;

            var featLayer = EsriUtils.getLayerByName(pMap, EditedItem.GetType().Name.Substring(3));
            var featClass = ((IFeatureLayer)featLayer).FeatureClass;

            IQueryFilter queryFilter = new QueryFilterClass();
            queryFilter.WhereClause =nameof(AM_AbstractFeature.featureID) + " =" + "'" + EditedItem.featureID + "'";

            var featCursor = featClass.Search(queryFilter, false);
            var feat = featCursor.NextFeature();

            if (feat is null || feat.Shape is null)
            {
                MessageBox.Show("Geometry is null", "Aerodrome", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            IEnvelope selectionFootprint = new EnvelopeClass();

            selectionFootprint.Union(feat.ShapeCopy.Envelope);

            pMxDoc.ActiveView.Extent = selectionFootprint;
            pMxDoc.ActiveView.Refresh();
        }
    }
}
