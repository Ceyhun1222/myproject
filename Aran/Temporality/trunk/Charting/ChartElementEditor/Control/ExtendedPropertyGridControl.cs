using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ChartElementEditor.ViewModel;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace ChartElementEditor.Control
{
    public class ExtendedPropertyGridControl : PropertyGrid
    {
        public static readonly DependencyProperty CurrentPropertiesProperty = 
            DependencyProperty.Register("CurrentProperties", typeof (List<ExtendedPropertyDefinition>), typeof (ExtendedPropertyGridControl),
            new PropertyMetadata(default(List<ExtendedPropertyDefinition>), OnCurrentPropertiesChanged));

        private static void OnCurrentPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ExtendedPropertyGridControl;
            if (instance==null) return;
            var newValue = e.NewValue as List<ExtendedPropertyDefinition>;

            //instance.AutoGenerateProperties = false;

            //instance.PropertyDefinitions = new PropertyDefinitionCollection();
            //instance.PropertyDefinitions.Clear();
            
            if (newValue==null) return;

            foreach (var prop in newValue)
            {
                instance.PropertyDefinitions.Add(prop.PropertyDefinition);
            }

            
            

            instance.Update();

        }

        public List<ExtendedPropertyDefinition> CurrentProperties
        {
            get { return (List<ExtendedPropertyDefinition>) GetValue(CurrentPropertiesProperty); }
            set { SetValue(CurrentPropertiesProperty, value); }
        }
    }
}
