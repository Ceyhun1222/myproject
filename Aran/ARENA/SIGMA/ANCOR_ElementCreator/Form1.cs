using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using ANCOR.MapCore;
using ANCOR.MapElements;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ANCOR;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        #region

        private Dictionary<string, AbstractChartElement> ChartElementsList = new Dictionary<string, AbstractChartElement>();

        #endregion

        //private AbstractChartElement acntElement = null;
        private AbstractChartElement SelectedChartElement;

        IActiveView pageView = null;
        IGraphicsContainer graphicsContainer = null;
        List<AbstractChartElement> lst = new List<AbstractChartElement>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {


            //////////////////////////////////
            AbstractChartElement acntElement;
            IPoint pnt = new PointClass();
            pnt.PutCoords((pageView.Extent as IArea).Centroid.X, (pageView.Extent as IArea).Centroid.Y);

            ILine3 ln = new LineClass();
            IPoint frm_pnt = new PointClass();
            frm_pnt.PutCoords((pageView.Extent as IArea).Centroid.X, (pageView.Extent as IArea).Centroid.Y);
            IPoint to_pnt = new PointClass();
            to_pnt.PutCoords((pageView.Extent as IArea).Centroid.X - 3, (pageView.Extent as IArea).Centroid.Y - 3);
            ln.FromPoint = frm_pnt;
            ln.ToPoint = to_pnt;

            object o = null;
            acntElement = null;
            IElement El = null;

            HelperClass.RemoveAllIElementsFromMap(graphicsContainer);

            switch (comboBox1.SelectedIndex)
            {
                case (0):
                    ChartElement_SimpleText SimpleText = new ChartElement_SimpleText("TextfromDataSource");
                    o = SimpleText.ConvertToIElement();
                    acntElement = SimpleText;

                    break;
                case (1):
                    ChartElement_BorderedText SimpleTextBorder = new ChartElement_BorderedText("TextfromDataSource");
                    o = SimpleTextBorder.ConvertToIElement();
                    acntElement = SimpleTextBorder;

                    break;
                case (2):
                    ChartElement_BorderedText_Collout SimpleTextBorderCollout = new ChartElement_BorderedText_Collout("TextfromDataSource");
                    SimpleTextBorderCollout.Anchor = new AncorPoint(pnt.X - 1, pnt.Y - 1);
                    o = SimpleTextBorderCollout.ConvertToIElement();
                    acntElement = SimpleTextBorderCollout;

                    break;

                case (3):
                    ChartElement_BorderedText_Collout_CaptionBottom TextBorderColloutCaptionBottom = new ChartElement_BorderedText_Collout_CaptionBottom("TextfromDataSource");
                    TextBorderColloutCaptionBottom.Anchor = new AncorPoint(pnt.X - 1, pnt.Y - 1);
                    o = TextBorderColloutCaptionBottom.ConvertToIElement();
                    acntElement = TextBorderColloutCaptionBottom;

                    break;

                case (4):
                    ChartElement_RouteDesignator RouteSign = new ChartElement_RouteDesignator("RoughtSign");
                    RouteSign.Anchor = new AncorPoint(pnt.X - 1, pnt.Y - 1);
                    o = RouteSign.ConvertToIElement();
                    acntElement = RouteSign;

                    break;

                case (5):
                    ChartElement_TextArrow TextArrow = new ChartElement_TextArrow("TextfromDataSource");
                    TextArrow.Anchor = new AncorPoint(pnt.X - 1, pnt.Y - 1);

                    o = TextArrow.ConvertToIElement();
                    acntElement = TextArrow;

                    break;

                case (6):
                    GraphicsChartElement_SafeArea SafeArea = new GraphicsChartElement_SafeArea(1200, distanceUOM.Feets.ToString(), "VOR");
                    SafeArea.LineColor = new AncorColor(0, 0, 0);
                    SafeArea.Sectors = new List<SafeArea_SectorDescription>();


                    SafeArea_SectorDescription sector1 = new SafeArea_SectorDescription(0, 180, 2000, "Feets");
                    SafeArea_SectorDescription sector2 = new SafeArea_SectorDescription(180, 360, 2500, "Feets");

                    SafeArea.Sectors.Add(sector1);
                    SafeArea.Sectors.Add(sector2);

                    SafeArea.Position = new AncorPoint(pnt.X, pnt.Y);

                    o = SafeArea.ConvertToIElement();
                    acntElement = SafeArea;


                    break;

                case (7):
                    GraphicsChartElement_TAA chertElement = new GraphicsChartElement_TAA();
                    chertElement.LineColor = new AncorColor(0, 0, 0);

                    chertElement.Position = new AncorPoint(pnt.X, pnt.Y);

                    o = chertElement.ConvertToIElement();
                    acntElement = chertElement;

                    break;

                case (8):
                    GraphicsChartElement_NorthArrow northAr = new GraphicsChartElement_NorthArrow("VAR 2.30 E -2014", hemiSphere.EasternHemisphere);
                    northAr.Position = new AncorPoint(pnt.X, pnt.Y);

                    o = northAr.ConvertToIElement();
                    acntElement = northAr;

                    break;

                case (9):
                    GraphicsChartElement_ScaleBar sclbr = new GraphicsChartElement_ScaleBar(graphicsContainer, pageView, false);
                    sclbr.Position = new AncorPoint(pnt.X, pnt.Y);
                    sclbr.CurrentMapScale = pageView.FocusMap.MapScale;
                    o = sclbr.ConvertToIElement();
                    acntElement = sclbr;

                    break;

                case (10):
                    ChartElement_BorderedText_Collout_CaptionBottom TextBorderColloutCaption = new ChartElement_BorderedText_Collout_CaptionBottom("InnerText");
                    TextBorderColloutCaption.BottomTextLine = null;
                    TextBorderColloutCaption.Border.FrameMargins = new AncorFrameMargins(-4, 0, 2, 2);
                    TextBorderColloutCaption.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = TextBorderColloutCaption.ConvertToIElement();
                    acntElement = TextBorderColloutCaption;

                    break;

                case (11):
                    ChartElement_SimpleText AmaText = new ChartElement_SimpleText("text1");

                    List<AncorChartElementWord> txtLine = AmaText.TextContents[0];
                    AncorChartElementWord wrd = new AncorChartElementWord("text2");//создаем слово
                    //wrd.TextValue = "value";
                    wrd.Font.Bold = true;
                    wrd.StartSymbol = new AncorSymbol("");
                    wrd.EndSymbol = new AncorSymbol("");
                    //wrd.Morse = true;
                    txtLine.Add(wrd);
                    //AmaText.TextContents.Add(txtLine);

                    o = AmaText.ConvertToIElement();
                    acntElement = AmaText;

                    break;

                case (12):
                    ChartElement_MarkerSymbol mrkr = new ChartElement_MarkerSymbol("");
                    mrkr.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = mrkr.ConvertToIElement();
                    acntElement = mrkr;

                    break;

                case (13):
                    ChartElement_SigmaCollout SigmaCollout = new ChartElement_SigmaCollout("InnerText");

                    SigmaCollout.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = SigmaCollout.ConvertToIElement();

                    ChartElement_SigmaCollout o1 = (ChartElement_SigmaCollout)SigmaCollout.Clone();

                    acntElement = SigmaCollout;

                    break;

                case (14):
                    ChartElement_Radial radText = new ChartElement_Radial("ABC", "80");
                    o = radText.ConvertToIElement();
                    acntElement = radText;

                    break;

                case (15):
                    ChartElement_SigmaCollout_Navaid SigmaCollout_Navaid = new ChartElement_SigmaCollout_Navaid("InnerText");
                    SigmaCollout_Navaid.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = SigmaCollout_Navaid.ConvertToIElement();

                    acntElement = SigmaCollout_Navaid;

                    break;

                case (16):
                    ChartElement_SigmaCollout_Designatedpoint SigmaCollout_Dpn = new ChartElement_SigmaCollout_Designatedpoint("InnerText");

                    SigmaCollout_Dpn.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = SigmaCollout_Dpn.ConvertToIElement();

                    acntElement = SigmaCollout_Dpn;

                    break;

                case (17):
                    ChartElement_SigmaCollout_Airspace SigmaCollout_Arsps = new ChartElement_SigmaCollout_Airspace("InnerText");
                    SigmaCollout_Arsps.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = SigmaCollout_Arsps.ConvertToIElement();

                    acntElement = SigmaCollout_Arsps;

                    break;

                case (18):
                    ChartElement_SigmaCollout_AccentBar SigmaCollout_AccentBar = new ChartElement_SigmaCollout_AccentBar("InnerText");
                    SigmaCollout_AccentBar.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = SigmaCollout_AccentBar.ConvertToIElement();

                    acntElement = SigmaCollout_AccentBar;

                    break;

                case (19):
                    ChartElement_ILSCollout SigmaCollout_ILS = new ChartElement_ILSCollout { Depth = 20, Length = 100, Width = 50, ILSStyle = SigmaIlsStyle.profileStyle1, Name  = "PC", Slope = 58, FillColor  = new AncorColor (20,20,20), IlsAnchorPoint = SigmaIlsAnchorPoint.LOC };
                    SigmaCollout_ILS.Anchor = new AncorPoint(pnt.X, pnt.Y);
                    o = SigmaCollout_ILS.ConvertToIElement();

                    acntElement = SigmaCollout_ILS;

                    break;
            }

            propertyGrid1.SelectedObject = acntElement;

            ChartElementsList.Add(acntElement.Id.ToString(), acntElement);


            El = (IElement)o;
            if (El is IGroupElement)
            {
                IGroupElement GrEl = El as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    if ((GrEl.get_Element(i).Geometry == null) || (GrEl.get_Element(i).Geometry.IsEmpty))
                        if (o is ChartElement_RouteDesignator) GrEl.get_Element(i).Geometry = ln;
                        else GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else
            {
                // if (El.Geometry == null) El.Geometry = new PointClass();
                El.Geometry = pnt;
            }




            IElementProperties3 ElProp = (IElementProperties3)El;
            ElProp.Name = ((AbstractChartElement)propertyGrid1.SelectedObject).Name;


            graphicsContainer.AddElement(El, 0);
            pageView.Refresh();
            axPageLayoutControl1.ActiveView.Refresh();

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;//comboBox1.Items.Count-1;
            pageView = axPageLayoutControl1.ActiveView;
            graphicsContainer = pageView.GraphicsContainer;


            axToolbarControl1.SetBuddyControl(axPageLayoutControl1);

        }


        void axPageLayoutControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IPageLayoutControlEvents_OnMouseDownEvent e)
        {
            graphicsContainer.Reset();

            IElement El = graphicsContainer.Next();
            while (El != null)
            {
                IElementProperties3 ElProp2 = (IElementProperties3)El;
                if (ElProp2.Name.StartsWith("AcntElement"))
                {
                    if (El.HitTest(e.pageX, e.pageY, 0))
                    {
                        ////////////////////////////////////////////////////////////////////////////////////////////

                        IElementInfo info = new IElementInfo(ElProp2.Name);

                        SelectedChartElement = ChartElementsList[info.AcntElementId];


                        switch (info.AcntElementType)
                        {
                            case ("ChartElement_SimpleText"):
                            case ("ChartElement_BorderedText"):
                                //                                SelectedChartElement._OnMouseDown(sender, e, pageView.ScreenDisplay, (ISymbol)(El as ITextElement).Symbol,500000);
                                break;
                            default:
                                break;
                        }

                        ////////////////////////////////////////////////////////////////////////////////////////////

                        break;
                    }
                }
                El = graphicsContainer.Next();
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {

            AbstractChartElement chertElement2 = (AbstractChartElement)lst[listBox1.SelectedIndex].Clone();

            object o = chertElement2.ConvertToIElement();

            IPoint pnt = new PointClass();
            pnt.PutCoords(6, 6);
            ILine3 ln = new LineClass();
            IPoint frm_pnt = new PointClass();
            frm_pnt.PutCoords((pageView.Extent as IArea).Centroid.X, (pageView.Extent as IArea).Centroid.Y);
            IPoint to_pnt = new PointClass();
            to_pnt.PutCoords((pageView.Extent as IArea).Centroid.X - 3, (pageView.Extent as IArea).Centroid.Y - 3);
            ln.FromPoint = frm_pnt;
            ln.ToPoint = to_pnt;


            IElement El = (IElement)o;

            ChartElementsList.Add(chertElement2.Id.ToString(), chertElement2);


            if (El is IGroupElement)
            {
                IGroupElement GrEl = El as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    if ((GrEl.get_Element(i).Geometry == null) || (GrEl.get_Element(i).Geometry.IsEmpty))
                        if (o is ChartElement_RouteDesignator) GrEl.get_Element(i).Geometry = ln;
                        else GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else El.Geometry = pnt;


            IElementProperties3 ElProp = (IElementProperties3)El;
            ElProp.Name = "AcntElement2";

            graphicsContainer.AddElement(El, 0);
            pageView.Refresh();
            axPageLayoutControl1.ActiveView.Refresh();

            graphicsContainer.Reset();
            int k = 0;
            IElement eee = graphicsContainer.Next();
            while (eee != null)
            {
                ElProp = (IElementProperties3)eee;
                if (ElProp.Name.StartsWith("AcntElement")) k++;

                eee = graphicsContainer.Next();
            }

            propertyGrid1.SelectedObject = chertElement2;


            button5_Click(sender, e);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshObject_IElement();
        }

        private void RefreshObject_IElement()
        {
            IElement El = null;
            IPoint pnt = new PointClass();
            pnt.PutCoords((pageView.Extent as IArea).Centroid.X, (pageView.Extent as IArea).Centroid.Y);


            HelperClass.RemoveIElementFromMap(graphicsContainer, ((AbstractChartElement)propertyGrid1.SelectedObject).Name);

            El = ((AbstractChartElement)propertyGrid1.SelectedObject).ConvertToIElement() as IElement;

            if (El is IGroupElement)
            {
                IGroupElement GrEl = El as IGroupElement;
                for (int i = 0; i <= GrEl.ElementCount - 1; i++)
                {
                    if ((GrEl.get_Element(i).Geometry == null) || (GrEl.get_Element(i).Geometry.IsEmpty)) GrEl.get_Element(i).Geometry = pnt as IGeometry;
                }
            }
            else El.Geometry = pnt;




            IElementProperties3 ElProp = (IElementProperties3)El;
            ElProp.Name = ((AbstractChartElement)propertyGrid1.SelectedObject).Name;

            graphicsContainer.AddElement(El, 0);
            pageView.Refresh();
            axPageLayoutControl1.ActiveView.Refresh();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(propertyGrid1.SelectedObject);
            lst.Add((AbstractChartElement)propertyGrid1.SelectedObject);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ///System.Diagnostics.Debug.WriteLine(listBox1.SelectedValue.GetType().Name);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // System.Diagnostics.Debug.WriteLine(listBox1.SelectedValue.GetType().Name);
            HelperClass.RemoveAllIElementsFromMap(graphicsContainer);
            propertyGrid1.SelectedObject = lst[listBox1.SelectedIndex];
            RefreshObject_IElement();

        }

        private void button4_Click(object sender, EventArgs e)
        {

            SaveFileDialog fd = new SaveFileDialog();

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                Chart_ObjectsList obj = new Chart_ObjectsList { ChartType = "enrt", List = lst };

                //Object obj = acntElement;

                XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
                MemoryStream strmMemSer = new MemoryStream();
                xmlSer.Serialize(strmMemSer, obj);



                byte[] byteArr = new byte[strmMemSer.Length];
                strmMemSer.Position = 0;
                int count = strmMemSer.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    strmMemSer.Close();
                    Console.WriteLine("Test Failed: Unable to read data from file");
                    return;
                }




                string FN = fd.FileName;
                if (File.Exists(FN)) File.Delete(FN);
                FileStream strmFileCompressed = new FileStream(FN, FileMode.CreateNew);
                strmMemSer.WriteTo(strmFileCompressed);
                strmMemSer.Close();
                strmFileCompressed.Close();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                listBox1.Items.Clear();

                lst = new List<AbstractChartElement>();

                string _file = fd.FileName;
                var fs = new System.IO.FileStream(_file, FileMode.Open);
                var byteArr = new byte[fs.Length];
                fs.Position = 0;
                var count = fs.Read(byteArr, 0, byteArr.Length);
                if (count != byteArr.Length)
                {
                    fs.Close();
                    Console.WriteLine(@"Test Failed: Unable to read data from file");
                }


                var strmMemSer = new MemoryStream();
                strmMemSer.Write(byteArr, 0, byteArr.Length);
                strmMemSer.Position = 0;

                var xmlSer = new XmlSerializer(typeof(Chart_ObjectsList));
                var prj = (Chart_ObjectsList)xmlSer.Deserialize(strmMemSer);

                foreach (var item in prj.List)
                {
                    listBox1.Items.Add(item);
                    lst.Add(item);
                }

                listBox1.SelectedIndex = 0;
            }
        }




    }
}
