using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESRI.ArcGIS.Carto;

namespace ChartTypeA.Utils
{
    class EsriElementFinder
    {
        public static IElement GetElementByname(string elementName)
        {
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            if (graphicsContainer != null)
            {
                graphicsContainer.Reset();
                var element = graphicsContainer.Next();

                while (element != null)
                {
                    var textElement = element as ITextElement;
                    if (textElement != null)
                    {
                        IElementProperties3 elemProperties = element as IElementProperties3;
                        if (elemProperties == null) continue;

                        if (elemProperties.Name.Equals(elementName))
                            return element;
                    }
                    element = graphicsContainer.Next();
                }
            }
            return null;
        }

    }
}
