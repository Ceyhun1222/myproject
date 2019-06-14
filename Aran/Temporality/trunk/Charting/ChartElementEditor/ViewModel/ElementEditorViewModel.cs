using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Controls;
using Aran.Temporality.Common.ArcGis;
using ChartElementEditor.Elements;
using ESRI.ArcGIS;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geometry;
using ImpromptuInterface;
using MvvmCore;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace ChartElementEditor.ViewModel
{


    public class ExtendedPropertyDefinition
    {
        private string _simpleProperty;
        public PropertyDefinition PropertyDefinition { get; set; }

        public string SimpleProperty
        {
            get { return _simpleProperty; }
            set
            {
                _simpleProperty = value;
                PropertyDefinition = new PropertyDefinition { TargetProperties = { SimpleProperty } };
            }
        }
    }

    public class ElementEditorViewModel : ViewModelBase 
    {
        private object _selectedObject;
      
        public object SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                OnPropertyChanged("SelectedObject");
            }
        }

        private List<ExtendedPropertyDefinition> _currentProperties;
        public List<ExtendedPropertyDefinition> CurrentProperties
        {
            get { return _currentProperties; }
            set
            {
                _currentProperties = value;
                OnPropertyChanged("CurrentProperties");
            }
        }


        public ElementEditorViewModel()
        {
        
            LicenseInitializer.Instance.InitializeApplication(
              new[] { esriLicenseProductCode.esriLicenseProductCodeStandard },
              new esriLicenseExtensionCode[] { });

            //do init esri
            var p = new PointClass();
        }


        public void Load()
        {
            //var p=new PointClass();
            var p = new Label();
            var props=new List<ExtendedPropertyDefinition>();
            props.Add(new ExtendedPropertyDefinition {SimpleProperty = "Width"});
            CurrentProperties = props;

          
            SelectedObject = p;
        }
    }

  
}
