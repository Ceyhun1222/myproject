using System;
using System.Windows;
using System.Windows.Controls;

namespace MapEnv.Toc
{
	public class TocTemplateSelector : DataTemplateSelector
	{
		public TocTemplateSelector()
		{
		}
		
		public DataTemplate AimSimpleTemplate { get; set; }

        public DataTemplate AimComplexTemplate { get; set; }

		public DataTemplate EsriFeatureTemplate { get; set; }

        public DataTemplate EsriRasterTemplate { get; set; }

		public override DataTemplate SelectTemplate (object item, DependencyObject container)
		{
			var tocItem = item as TocItem;
			
			if (tocItem != null)
			{
				//var elem = container as FrameworkElement;

                if (tocItem.TocType == TocItemType.AimSimple)
                    return AimSimpleTemplate;
                else if (tocItem.TocType == TocItemType.AimComplex)
                    return AimComplexTemplate;
                else if (tocItem.TocType == TocItemType.EsriFeature)
                    return EsriFeatureTemplate;
                else if (tocItem.TocType == TocItemType.EsriRaster)
                    return EsriRasterTemplate;
			}
			
			return null;
		}
	}
}
