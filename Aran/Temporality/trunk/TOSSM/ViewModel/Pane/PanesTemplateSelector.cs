using System.Windows;
using System.Windows.Controls;
using TOSSM.ViewModel.Document;
using TOSSM.ViewModel.Document.Editor;
using TOSSM.ViewModel.Document.Evolution;
using TOSSM.ViewModel.Document.Graph;
using TOSSM.ViewModel.Document.Relations;
using TOSSM.ViewModel.Document.Slot;
using TOSSM.ViewModel.Tool;
using TOSSM.ViewModel.Tool.PropertyPrecision;

namespace TOSSM.ViewModel.Pane
{
    class PanesTemplateSelector : DataTemplateSelector
    {
        #region Templates

        public DataTemplate EraserTemplate { get; set; }
        public DataTemplate SlotViewerTemplate { get; set; }
        public DataTemplate RelationsTemplate { get; set; } 
        public DataTemplate InformerTemplate { get; set; } 
        public DataTemplate SlotSelectorTemplate { get; set; } 
        public DataTemplate GeoIntersectionTemplate { get; set; } 
        public DataTemplate MyAccountTemplate { get; set; } 
        public DataTemplate RelationFinderTemplate { get; set; } 
        public DataTemplate UserManagerTemplate { get; set; } 
        public DataTemplate BusinessRulesManagerTemplate { get; set; } 
        public DataTemplate SlotValidationReportTemplate { get; set; }
        public DataTemplate RelationExplorerTemplate { get; set; } 
        public DataTemplate FeatureSelectorTemplate { get; set; } 
        public DataTemplate FeaturePresenterTemplate { get; set; } 
        public DataTemplate FeatureEditorTemplate { get; set; } 
        public DataTemplate EvolutionViewerTemplate { get; set; }
        public DataTemplate FeatureDependencyManagerTemplate { get; set; }
        public DataTemplate FeatureDependencyReportTemplate { get; set; }
        public DataTemplate PropertyPrecisionEditorTemplate { get; set; }
        public DataTemplate ImportToolTemplate { get; set; }
        public DataTemplate ExportToolTemplate { get; set; }
        public DataTemplate AIMSLToolTemplate { get; set; }
        public DataTemplate SlotMergeTemplate { get; set; }
        public DataTemplate NotamPresenterTemplate { get; set; }

        public DataTemplate DataSourceTemplateManagerTemplate { get; set; }
        public DataTemplate LogViewerTemplate { get; set; }

        
        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ExportToolViewModel)
                return ExportToolTemplate;
            

            if (item is PrecisionEditorToolViewModel)
                return PropertyPrecisionEditorTemplate;

            if (item is LogViewerToolViewModel)
                return LogViewerTemplate;

            if (item is DataSourceTemplateManagerViewModel)
                return DataSourceTemplateManagerTemplate;

            if (item is ImportToolViewModel)
                return ImportToolTemplate;

            if (item is FeatureDependencyReportDocViewModel)
                return FeatureDependencyReportTemplate;

            if (item is FeatureDependencyManagerToolViewModel)
                return FeatureDependencyManagerTemplate;

            if (item is SlotValidationReportViewModel)
                return SlotValidationReportTemplate;

            if (item is BusinessRulesManagerToolViewModel)
                return BusinessRulesManagerTemplate;

            if (item is RelationExplorerDocViewModel)
                 return RelationExplorerTemplate;

            if (item is RelationFinderToolViewModel)
                return RelationFinderTemplate;

            if (item is SlotContentDocViewModel)
                return SlotViewerTemplate;

            if (item is MyAccountToolViewModel)
                return MyAccountTemplate;

            if (item is UserManagerToolViewModel)
                return UserManagerTemplate;

            if (item is SlotSelectorToolViewModel)
                return SlotSelectorTemplate;

            if (item is GeoIntersectionDocViewModel)
                return GeoIntersectionTemplate;

            if (item is EraserDocViewModel)
                return EraserTemplate;

            if (item is RelationsDocViewModel)
                return RelationsTemplate;

            if (item is FeatureSelectorToolViewModel)
                return FeatureSelectorTemplate;

            if (item is InformerToolViewModel)
                return InformerTemplate;

            if (item is FeaturePresenterToolViewModel)
                return FeaturePresenterTemplate;

            if (item is FeatureSubEditorDocViewModel)
                return FeatureEditorTemplate;

            if (item is EvolutionDocViewModel)
                return EvolutionViewerTemplate;

            if (item is AIMSLToolViewModel)
                return AIMSLToolTemplate;

            if (item is SlotMergeViewModel)
                return SlotMergeTemplate;

            if (item is NotamPresenterViewModel)
                return NotamPresenterTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
