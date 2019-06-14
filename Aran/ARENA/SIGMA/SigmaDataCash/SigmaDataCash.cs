using ANCOR;
using ANCOR.MapElements;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Drawing;
using ChartCompare;
using PDM;

namespace SigmaChart
{
    public static class SigmaDataCash
    {
        //Данная переменная статического класса будет доступна откуда угодно в пределах проекта
        public static List<object> ChartElementList;
        public static List<AbstractChartElement> prototype_anno_lst;
        public static Dictionary<string, object> AnnotationFeatureClassList;
        public static Dictionary<string, IFeatureClass> AnnotationLinkedGeometryList;
        public static System.Windows.Forms.TreeView ChartElementsTree;
        public static System.Windows.Forms.PropertyGrid AncorPropertyGrid;
        public static System.Windows.Forms.PropertyGrid MirrorPropertyGrid;
        public static IWorkspaceEdit environmentWorkspaceEdit;
        public static bool MultiSelectFlag = false;
        public static List<AbstractChartElement> SelectedChartElements;
        public static int SigmaChartType;
        public static int UpdateState = 0;
        public static List<PDMObject> oldPdmList;
        public static List<PDMObject> newPdmList;
        public static List<DetailedItem> ChangeList;
        public static List<string> Report;

        public static void GetProtoTypeAnnotation(string pathToTemplateFileXML)
        {
            try
            {
                //MessageBox.Show("");
                SigmaDataCash.prototype_anno_lst = new List<AbstractChartElement>();
                
                //string TmpFolder = System.IO.Path.GetTempPath() + System.IO.Path.GetFileName(pathToTemplateFileXML);
                //System.IO.File.Copy(pathToTemplateFileXML, TmpFolder);

                var fs = new System.IO.FileStream(pathToTemplateFileXML, FileMode.Open);
                var byteArr = new byte[fs.Length];
                fs.Position = 0;
                var count = fs.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    fs.Close();
                    Console.WriteLine(@"Test Failed: Unable to read data from file");
                }
                fs.Dispose();

                var strmMemSer = new MemoryStream();
                strmMemSer.Write(byteArr, 0, byteArr.Length);
                strmMemSer.Position = 0;

                var xmlSer = new XmlSerializer(typeof(Chart_ObjectsList));
                var prj = (Chart_ObjectsList)xmlSer.Deserialize(strmMemSer);

                SigmaDataCash.prototype_anno_lst.AddRange(prj.List);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.InnerException.ToString());
                //System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            }
        }

        public static AbstractChartElement HighLightChartElement(string ElementID, string objectReflection ="")
        {
            bool ok = false;
            AbstractChartElement res = null;
            if (ChartElementsTree == null) return null;
            foreach (TreeNode ParentNode in ChartElementsTree.Nodes)
            {
                foreach (TreeNode item in ParentNode.Nodes)
                {
                    if (item.Tag == null) continue;
                    if (((AbstractChartElement)item.Tag).Id.ToString().CompareTo(ElementID) == 0)
                    {

                        ChartElementsTree.SelectedNode = item;
                        ChartElementsTree.SelectedNode.Expand();
                        ChartElementsTree.Select();
                        ChartElementsTree.Focus();

                        res = (AbstractChartElement)item.Tag;
                        if (objectReflection.Trim().Length > 0)
                        {
                            AbstractChartElement mirrorObj = (AbstractChartElement)DeserializeReflection<AbstractChartElement>(objectReflection, ((AbstractChartElement)item.Tag).GetType().Name.ToString());
                            MirrorPropertyGrid.SelectedObject = mirrorObj;
                            res = mirrorObj;
                        }

                        ok = true;

                        break;
                    }
                }
                if (ok) break;
            }

            return res;

        }

        private static object DeserializeReflection<T>(string objectReflection, string ObjType)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            switch (ObjType)
            {
                case ("ChartElement_SimpleText"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_SimpleText));
                    break;
                case ("ChartElement_RouteDesignator"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_RouteDesignator));
                    break;
                case ("ChartElement_BorderedText"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_BorderedText));
                    break;
                case ("ChartElement_BorderedText_Collout_CaptionBottom"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_BorderedText_Collout_CaptionBottom));
                    break;
                case ("ChartElement_BorderedText_Collout"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_BorderedText_Collout));
                    break;
                case ("ChartElement_SigmaCollout_Navaid"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_SigmaCollout_Navaid));
                    break;
                case ("ChartElement_MarkerSymbol"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_MarkerSymbol));
                    break;
                case ("ChartElement_TextArrow"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_TextArrow));
                    break;
                case ("ChartElement_Radial"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_Radial));
                    break;
                case ("ChartElement_SigmaCollout_Designatedpoint"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_SigmaCollout_Designatedpoint));
                    break;
                case ("ChartElement_SigmaCollout_Airspace"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_SigmaCollout_Airspace));
                    break;
                case ("ChartElement_SigmaCollout_AccentBar"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_SigmaCollout_AccentBar));
                    break;
                case ("ChartElement_ILSCollout"):
                    xmlSerializer = new XmlSerializer(typeof(ChartElement_ILSCollout));
                    break;
                default:
                    break;
            }


            
            StringReader textReader = new StringReader(objectReflection);
            return xmlSerializer.Deserialize(textReader);
        }

        public static void PutOutPropertyGrid()
        {
            if (ChartElementsTree == null) return;
            ChartElementsTree.SelectedNode = null;
            
            ChartElementsTree.Focus();
            if (ChartElementsTree.SelectedNode == null) return;
            if (((AbstractChartElement)ChartElementsTree.SelectedNode.Tag!=null) && ((AbstractChartElement)ChartElementsTree.SelectedNode.Tag).Placed)
            {
                ChartElementsTree.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Regular);
                ChartElementsTree.SelectedNode.ForeColor = System.Drawing.SystemColors.ControlText;
            }
            else
            {
                ChartElementsTree.SelectedNode.NodeFont = new System.Drawing.Font(new FontFamily("Times New Roman"), 10, FontStyle.Italic | FontStyle.Strikeout);
            }

            AncorPropertyGrid.SelectedObject = null;
            MirrorPropertyGrid.SelectedObject = null;
        }
    }


}
