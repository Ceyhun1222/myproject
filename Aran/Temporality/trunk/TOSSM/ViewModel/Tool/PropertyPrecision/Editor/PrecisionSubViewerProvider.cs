using Aran.Aim.PropertyPrecision;
using System.Windows;
using TOSSM.Control;
using TOSSM.View.Tool;
using TOSSM.ViewModel.Tool.PropertyPrecision.Single;

namespace TOSSM.ViewModel.Tool.PropertyPrecision.Editor
{
    public class PrecisionSubViewerProvider
    {
        public static HierarchyControl GetSubViewer(HierarchyControl subViewer,
                                                 ComplexPropertyPrecisionViewModel selectedProperty,
                                                 HierarchyControl currentViewer)
        {
            if (selectedProperty == null) return null;

            if (!(subViewer is PropertyPrecisionSubEditor))
            {
                subViewer = new PropertyPrecisionSubEditor {DataContext = new PrecisionSubEditorViewModel()};

                var subModel = (PrecisionSubEditorViewModel)subViewer.DataContext;
                subModel.ParentViewer = currentViewer;
               
                var propConfigurations = selectedProperty.ParentModel.PropertyConfiguration.ObjectConfiguration.Properties;
                PropertyConfiguration propertyConfiguration;
                if (!propConfigurations.TryGetValue(selectedProperty.PropInfo.Index, out propertyConfiguration))
                {
                    propertyConfiguration=new ComplexPropertyConfiguration();
                    propConfigurations[selectedProperty.PropInfo.Index] = propertyConfiguration;
                }
                subModel.PropertyConfiguration = (ComplexPropertyConfiguration)propertyConfiguration;

                subModel.EditedFeature = selectedProperty.PropInfo.PropType.Index;
            }
            else
            {
                var subModel = (PrecisionSubEditorViewModel)subViewer.DataContext;

                var propConfigurations = selectedProperty.ParentModel.PropertyConfiguration.ObjectConfiguration.Properties;
                PropertyConfiguration propertyConfiguration;
                if (!propConfigurations.TryGetValue(selectedProperty.PropInfo.Index, out propertyConfiguration))
                {
                    propertyConfiguration = new ComplexPropertyConfiguration();
                    propConfigurations[selectedProperty.PropInfo.Index] = propertyConfiguration;
                }
                subModel.PropertyConfiguration = (ComplexPropertyConfiguration)propertyConfiguration;

                subModel.EditedFeature = selectedProperty.PropInfo.PropType.Index;
            }

            subViewer.Visibility = Visibility.Visible;
            return subViewer;
        }
    }
}
