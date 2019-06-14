using ChartTypeA.Models;
using ESRI.ArcGIS.Carto;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChartTypeA.Utils
{
    class DecDistancesSaver
    {
        public static void SaveDecDistances(RwyDirWrapper leftRwyDir, RwyDirWrapper rightRwyDir)
        {
            var graphicsContainer = GlobalParams.HookHelper.PageLayout as IGraphicsContainer;

            graphicsContainer.Reset();
            var element = graphicsContainer.Next();

            while (element != null)
            {
                if (element is IGroupElement)
                {
                    IElementProperties3 elemProperties = element as IElementProperties3;
                    if (elemProperties.Name == "DecDistances")
                    {
                        var groupElement = element as IGroupElement;
                        for (int i = 0; i < groupElement.ElementCount; i++)
                        {
                            var textElement = groupElement.Element[i] as ITextElement;
                            if (textElement != null)
                            {
                                var textElementProperties = textElement as IElementProperties3;
                                if (textElementProperties != null)
                                    switch (textElementProperties.Name)
                                    {
                                        case "LeftRwyDir":
                                            textElement.Text = leftRwyDir != null ? leftRwyDir.Name : "";
                                            break;
                                        case "RightRwyDir":
                                            textElement.Text = rightRwyDir != null ? rightRwyDir.Name : "";
                                            break;
                                        case "LeftTora":
                                            textElement.Text = leftRwyDir?.TORA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "RightTora":
                                            textElement.Text = rightRwyDir?.TORA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "LeftToda":
                                            textElement.Text = leftRwyDir?.TODA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "RightToda":
                                            textElement.Text = rightRwyDir?.TODA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "LeftLda":
                                            textElement.Text = leftRwyDir?.LDA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "RightLda":
                                            textElement.Text = rightRwyDir?.LDA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "LeftAsda":
                                            textElement.Text = leftRwyDir?.ASDA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                        case "RightAsda":
                                            textElement.Text = rightRwyDir?.ASDA.ToString(CultureInfo.InvariantCulture) ?? "";
                                            break;
                                    }
                            }
                        }
                    }
                }
                element = graphicsContainer.Next();
            }
        }

    }


}
