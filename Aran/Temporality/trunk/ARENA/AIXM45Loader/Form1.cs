using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ARINC_DECODER_CORE;
using AreaManager;
using System.Xml.Linq;
using ARINC_Types;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geometry;

namespace AIXM45Loader
{
    public partial class Form1 : Form
    {
        private List<AIXM45_Object> AIXM45_ObjectsList;
        AranSupport.Utilitys util;
        XDocument xdoc;
        AreaInfo Area;

        public Form1()
        {
            InitializeComponent();

            label_Step1.Text = wizardTabControl1.TabPages[0].Text;
            label_Step2.Text = wizardTabControl1.TabPages[1].Text;
            label_Step3.Text = wizardTabControl1.TabPages[2].Text;

            treeView3.ExpandAll();

            AIXM45_ObjectsList = new List<AIXM45_Object>();
            util = new AranSupport.Utilitys();

        }

        private void Next_button_Click(object sender, EventArgs e)
        {
            HideLabelStep(wizardTabControl1.SelectedIndex);


            wizardTabControl1.SelectedIndex++;


            Next_button.Enabled = !(wizardTabControl1.SelectedIndex == wizardTabControl1.TabPages.Count - 1);
            Prev_button.Enabled = true;
            ShowLabelStep(wizardTabControl1.SelectedIndex);

            button1.Enabled = !Next_button.Enabled;

            if (wizardTabControl1.SelectedIndex == (wizardTabControl1.TabPages.Count - 1))
            {
                SaveAreaXmlfile();
                switch (System.IO.Path.GetExtension(textBox1.Text))
                {
                        
                    case (".xml"):
                        DecodeAIXM45_File();
                        break;

                    case (".mdb"):
                        DecodeMDB45_File();

                        break;
                }



                button1.Enabled = wizardTabControl1.SelectedIndex == (wizardTabControl1.TabPages.Count - 1);


                DataTable _Table = ConvertToDataTable(AIXM45_ObjectsList);
                dataGridView1.DataSource = _Table;

            }
        }

        private void SaveAreaXmlfile()
        {

            string PathToSpecificationFile = Static_Proc.GetPathToSpecificationFile();
            string PathToRegionsFile = Static_Proc.GetPathToRegionsFile();

            //if (textBox3.Text.Length == 4) checkBox6.Checked = true;

            AreaInfo area = new AreaInfo();
            area.CountryName = comboBox1.Text;
            area.FirstLetter = comboBox3.Text;
            area.Region = comboBox2.Text;
            area.Reference_ADHP.ICAO_CODE = textBox3.Text.Trim();
            AreaUtils.WriteToAreaFile(PathToRegionsFile, PathToSpecificationFile, area);

        }

        private void DecodeAIXM45_File()
        {

            List<string> ListOfSectionsType = new List<string>();

            #region выбор типов записей

            foreach (TreeNode Nd in treeView3.Nodes)
            {
                if ((Nd.Checked) && (Nd.Tag != null) && (Nd.Tag.ToString().Length > 0))
                {
                    string[] TAGS = Nd.Tag.ToString().Split(',');

                    foreach (string TAG in TAGS)
                        if (!ListOfSectionsType.Contains(TAG)) ListOfSectionsType.Add(TAG);
                }


                if (Nd.Nodes.Count > 0)
                {
                    foreach (TreeNode NdTags in Nd.Nodes)
                    {
                        if ((NdTags.Checked) && (NdTags.Tag != null) && (NdTags.Tag.ToString().Length > 0))
                        {
                            string[] TAGS = NdTags.Tag.ToString().Split(',');

                            foreach (string TAG in TAGS)
                                if (!ListOfSectionsType.Contains(TAG)) ListOfSectionsType.Add(TAG);
                        }
                    }
                }
            }

            #endregion


            xdoc = XDocument.Load(textBox1.Text);
            

            foreach (string SectionsType in ListOfSectionsType)
            {
                switch (SectionsType)
                {
                    case("PA"):

                        #region

                        var aixm45_AhpList = from aixm45_obj in xdoc.Descendants("Ahp")
                                             where aixm45_obj.Element("AhpUid") != null
                                             select
                                                 new AIXM45_Airport
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_MID = aixm45_obj.Element("AhpUid").Attribute("mid") != null ? aixm45_obj.Element("AhpUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     TxtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     R_codeId = aixm45_obj.Element("AhpUid") != null ? aixm45_obj.Element("AhpUid").Value : "",
                                                     CodeIata = aixm45_obj.Element("codeIata") != null ? aixm45_obj.Element("codeIata").Value : "",
                                                     CodeIcao = aixm45_obj.Element("codeIcao") != null ? aixm45_obj.Element("codeIcao").Value : "",
                                                     CodeType = aixm45_obj.Element("codeType")!=null?  aixm45_obj.Element("codeType").Value: "",
                                                     TxtDescrRefPt = aixm45_obj.Element("txtDescrRefPt")!=null?  aixm45_obj.Element("txtDescrRefPt").Value: "",
                                                     GeoLat = aixm45_obj.Element("geoLat") != null ? aixm45_obj.Element("geoLat").Value : "",
                                                     GeoLong = aixm45_obj.Element("geoLong") != null ? aixm45_obj.Element("geoLong").Value : "",
                                                     ValGeoAccuracy = (aixm45_obj.Element("valGeoAccuracy") != null) ? Double.Parse( aixm45_obj.Element("valGeoAccuracy").Value) : 0,
                                                     UomGeoAccuracy = aixm45_obj.Element("uomGeoAccuracy") != null ? aixm45_obj.Element("uomGeoAccuracy").Value : "",
                                                     ValElev = (aixm45_obj.Element("valElev") != null) ? Double.Parse(aixm45_obj.Element("valElev").Value) : 0,
                                                     UomDistVer = aixm45_obj.Element("uomDistVer") != null ? aixm45_obj.Element("uomDistVer").Value : "",
                                                     ValElevAccuracy = (aixm45_obj.Element("valElevAccuracy") != null) ? Double.Parse(aixm45_obj.Element("valElevAccuracy").Value) : 0,
                                                     TxtNameCitySer = aixm45_obj.Element("txtNameCitySer") != null ? aixm45_obj.Element("txtNameCitySer").Value : "",
                                                     //= aixm45_obj.Element("txtDescrSite")!=null?  aixm45_obj.Element("txtDescrSite").Value: "",
                                                     ValMagVar = aixm45_obj.Element("valMagVar") != null ? Double.Parse( aixm45_obj.Element("valMagVar").Value) : 0,
                                                     //= aixm45_obj.Element("dateMagVar")!=null?  aixm45_obj.Element("dateMagVar").Value: "",
                                                     //= aixm45_obj.Element("valMagVarChg")!=null?  aixm45_obj.Element("valMagVarChg").Value: "",
                                                     //= aixm45_obj.Element("valRefT")!=null?  aixm45_obj.Element("valRefT").Value: "",
                                                     //= aixm45_obj.Element("uomRefT")!=null?  aixm45_obj.Element("uomRefT").Value: "",
                                                     //= aixm45_obj.Element("txtNameAdmin")!=null?  aixm45_obj.Element("txtNameAdmin").Value: "",
                                                     //= aixm45_obj.Element("txtDescrAcl")!=null?  aixm45_obj.Element("txtDescrAcl").Value: "",
                                                     //= aixm45_obj.Element("txtDescrSryPwr")!=null?  aixm45_obj.Element("txtDescrSryPwr").Value: "",
                                                     //= aixm45_obj.Element("txtDescrLdi")!=null?  aixm45_obj.Element("txtDescrLdi").Value: "",
                                                     ValTransitionAlt = aixm45_obj.Element("valTransitionAlt") != null ?  Double.Parse(aixm45_obj.Element("valTransitionAlt").Value) : 0,
                                                     UomTransitionAlt = aixm45_obj.Element("uomTransitionAlt") != null ? aixm45_obj.Element("uomTransitionAlt").Value : "",
                                                     //= aixm45_obj.Element("txtRmk")!=null?  aixm45_obj.Element("txtRmk").Value: "",

                                                     #endregion
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_AhpList);

                        break;

                    case("PG"):

                        var aixm45_RwyList = from aixm45_obj in xdoc.Descendants("Rwy")
                                             where aixm45_obj.Element("RwyUid") != null
                                             select
                                                 new AIXM45_RWY
                                                 {
                                                     #region
                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("RwyUid").Attribute("mid") != null ? aixm45_obj.Element("RwyUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_txtDesig = aixm45_obj.Element("RwyUid").Element("txtDesig") != null ? aixm45_obj.Element("RwyUid").Element("txtDesig").Value : "",
                                                     R_AHPmid = aixm45_obj.Element("RwyUid").Element("AhpUid").Attribute("mid") != null ? aixm45_obj.Element("RwyUid").Element("AhpUid").Attribute("mid").Value : "",
                                                     ValLen = (aixm45_obj.Element("valLen") != null) ? Double.Parse(aixm45_obj.Element("valLen").Value) : 0,
                                                     ValWid = (aixm45_obj.Element("valWid") != null) ? Double.Parse(aixm45_obj.Element("valWid").Value) : 0,
                                                     UomDimRwy =(aixm45_obj.Element("uomDimRwy") != null) ? aixm45_obj.Element("uomDimRwy").Value : "",
                                                     ValLenStrip = (aixm45_obj.Element("valLenStrip") != null) ? Double.Parse(aixm45_obj.Element("valLenStrip").Value) : 0,
                                                     ValWidStrip = (aixm45_obj.Element("valWidStrip") != null) ? Double.Parse(aixm45_obj.Element("valWidStrip").Value) : 0,
                                                     UomDimStrip = (aixm45_obj.Element("uomDimStrip") != null) ? aixm45_obj.Element("uomDimStrip").Value : "",
                                                     #endregion
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_RwyList);


                        var aixm45_ThrList = from aixm45_obj in xdoc.Descendants("Rdn")
                                             where aixm45_obj.Element("RdnUid") != null
                                             select
                                                 new AIXM45_RDN
                                                 {
                                                     #region

                                                     ID =Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("RdnUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_AhpMid = aixm45_obj.Element("RdnUid").Element("RwyUid").Element("AhpUid").Attribute("mid") != null ? aixm45_obj.Element("RdnUid").Element("RwyUid").Element("AhpUid").Attribute("mid").Value : "",
                                                     R_RWYmid = aixm45_obj.Element("RdnUid").Element("RwyUid").Attribute("mid") !=null ? aixm45_obj.Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                     R_txtDesig = aixm45_obj.Element("RdnUid") != null ? aixm45_obj.Element("RdnUid").Element("txtDesig").Value : "",
                                                     GeoLat = aixm45_obj.Element("geoLat") != null ?  aixm45_obj.Element("geoLat").Value : "",
                                                     GeoLong = aixm45_obj.Element("geoLong") != null ? aixm45_obj.Element("geoLong").Value : "",
                                                     ValTrueBrg = aixm45_obj.Element("valTrueBrg") != null ? Double.Parse( aixm45_obj.Element("valTrueBrg").Value) : 0,
                                                     ValMagBrg = aixm45_obj.Element("valMagBrg") != null ? Double.Parse( aixm45_obj.Element("valMagBrg").Value) : 0,
                                                     ValElevTdz = aixm45_obj.Element("valElevTdz") != null ? Double.Parse(aixm45_obj.Element("valElevTdz").Value) : 0,
                                                     UomElevTdz = aixm45_obj.Element("uomElevTdz") != null ? aixm45_obj.Element("uomElevTdz").Value : "",
                                                     
                                                     //INFO_AIRTRACK = aixm45_obj.Element("RdnUid").Element("RwyUid").Attribute("mid") != null ? aixm45_obj.Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                     //Magnetic_Bearing = aixm45_obj.Element("valMagBrg") != null ? Double.Parse(aixm45_obj.Element("valMagBrg").Value) : 0,
                                                     //RDN_TXT_DESIG = aixm45_obj.Element("RdnUid") != null ? aixm45_obj.Element("RdnUid").Element("txtDesig").Value : "",
                                                     //True_Bearing = aixm45_obj.Element("valTrueBrg") != null ? Double.Parse(aixm45_obj.Element("valTrueBrg").Value) : 0,
                                                     //ThrElev_M = (aixm45_obj.Element("uomElevTdz") != null) && (aixm45_obj.Element("valElevTdz") != null) ? util.ConvertValueToMeter(aixm45_obj.Element("valElevTdz").Value, aixm45_obj.Element("uomElevTdz").Value) : 0,
                                                     #endregion
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_ThrList);


                        var aixm45_RcpList = from aixm45_obj in xdoc.Descendants("Rcp")
                                             where aixm45_obj.Element("RcpUid") != null
                                             select
                                                 new AIXM45_RCP
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("RcpUid").Attribute("mid") != null ? aixm45_obj.Element("RcpUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_RWYMID = aixm45_obj.Element("RcpUid").Element("RwyUid").Attribute("mid") != null ? aixm45_obj.Element("RcpUid").Element("RwyUid").Attribute("mid").Value : "",
                                                     R_geoLat = aixm45_obj.Element("RcpUid").Element("geoLat") != null ? aixm45_obj.Element("RcpUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("RcpUid").Element("geoLong") != null ? aixm45_obj.Element("RcpUid").Element("geoLong").Value : "",
                                                     valElev = aixm45_obj.Element("valElev") != null ? Double.Parse(aixm45_obj.Element("valElev").Value) : 0,
                                                     uomDistVer = aixm45_obj.Element("uomDistVer") != null ? aixm45_obj.Element("uomDistVer").Value : "",

                                                     #endregion
                                                 };

                                                AIXM45_ObjectsList.AddRange(aixm45_RcpList);

                        var aixm45_RddList = from aixm45_obj in xdoc.Descendants("Rdd")
                                                where aixm45_obj.Element("RddUid") != null
                                                select
                                                    new AIXM45_RDN_DECLARED_DISTANCE
                                                    {
                                                        #region

                                                        ID = Guid.NewGuid().ToString(),
                                                        R_mid = aixm45_obj.Element("RddUid").Attribute("mid") != null ? aixm45_obj.Element("RddUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                        R_codeType = aixm45_obj.Element("RddUid").Element("codeType") != null ? aixm45_obj.Element("RddUid").Element("codeType").Value : "",
                                                        R_RdnMid = aixm45_obj.Element("RddUid").Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("RddUid").Element("RdnUid").Attribute("mid").Value : "",
                                                        R_RdnTxtDesid = aixm45_obj.Element("RddUid").Element("RdnUid").Element("txtDesig") != null ? aixm45_obj.Element("RddUid").Element("RdnUid").Element("txtDesig").Value : "",
                                                        R_RWYMid = aixm45_obj.Element("RddUid").Element("RdnUid").Element("RwyUid").Attribute("mid") != null ? aixm45_obj.Element("RddUid").Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                        valDist = aixm45_obj.Element("valDist") !=null ? Double.Parse(aixm45_obj.Element("valDist").Value) : 0,
                                                        uomDist = aixm45_obj.Element("uomDist") != null ? aixm45_obj.Element("uomDist").Value : "",

                                                        #endregion
                                                    };

                        AIXM45_ObjectsList.AddRange(aixm45_RddList);

                         var aixm45_SwyList = from aixm45_obj in xdoc.Descendants("Swy")
                                                where aixm45_obj.Element("SwyUid") != null
                                                select
                                                    new AIXM45_RDN_SWY
                                                    {
                                                        #region

                                                        ID = Guid.NewGuid().ToString(),
                                                        R_mid = aixm45_obj.Element("SwyUid").Attribute("mid") != null ? aixm45_obj.Element("SwyUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                        R_RdnMid = aixm45_obj.Element("SwyUid").Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("SwyUid").Element("RdnUid").Attribute("mid").Value : "",
                                                        R_RdnTxtDesid = aixm45_obj.Element("SwyUid").Element("RdnUid").Element("txtDesig") != null ? aixm45_obj.Element("SwyUid").Element("RdnUid").Element("txtDesig").Value : "",
                                                        R_RWYMid = aixm45_obj.Element("SwyUid").Element("RdnUid").Element("RwyUid").Attribute("mid") != null ? aixm45_obj.Element("SwyUid").Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                        uomDim = aixm45_obj.Element("uomDim") != null ? aixm45_obj.Element("uomDim").Value : "",
                                                        valLen = aixm45_obj.Element("valLen") != null ? Double.Parse(aixm45_obj.Element("valLen").Value) : 0,
                                                        valWid = aixm45_obj.Element("valWid") != null ? Double.Parse(aixm45_obj.Element("valWid").Value) : 0,

                                                        #endregion
                                                    };

                         AIXM45_ObjectsList.AddRange(aixm45_SwyList);


                         var aixm45_RPA_CWY_List = from aixm45_obj in xdoc.Descendants("Rpa")
                                                   where aixm45_obj.Element("RpaUid") != null && aixm45_obj.Element("RpaUid").Element("codeType") != null && aixm45_obj.Element("RpaUid").Element("codeType").Value.CompareTo("CWY") == 0
                                                   select
                                                       new AIXM45_RPA
                                                       {
                                                           ID = Guid.NewGuid().ToString(),
                                                           R_mid = aixm45_obj.Element("RpaUid").Attribute("mid") != null ? aixm45_obj.Element("RpaUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                           R_RdnMid = aixm45_obj.Element("RpaUid").Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("RpaUid").Element("RdnUid").Attribute("mid").Value : "",
                                                           R_RdnTxtDesid = aixm45_obj.Element("RpaUid").Element("RdnUid").Element("txtDesig") != null ? aixm45_obj.Element("RpaUid").Element("RdnUid").Element("txtDesig").Value : "",
                                                           R_RWYMid = aixm45_obj.Element("RpaUid").Element("RdnUid").Element("RwyUid").Attribute("mid") != null ? aixm45_obj.Element("RpaUid").Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                           UomDim = aixm45_obj.Element("uomDim") != null ? aixm45_obj.Element("uomDim").Value : "",
                                                           valLen = aixm45_obj.Element("valLen") != null ? Double.Parse(aixm45_obj.Element("valLen").Value) : 0,
                                                           valWid = aixm45_obj.Element("valWid") != null ? Double.Parse(aixm45_obj.Element("valWid").Value) : 0,
                                                           R_codeType = "CWY",
                                                       };

                         AIXM45_ObjectsList.AddRange(aixm45_RPA_CWY_List);


                    #endregion

                        break;

                    case ("PI"):

                        #region

                        var aixm45_IlsList = from aixm45_obj in xdoc.Descendants("Ils")
                                             where aixm45_obj.Element("IlsUid") != null
                                             select
                                                 new AIXM45_ILS
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("IlsUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_RdnMid = aixm45_obj.Element("IlsUid").Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Attribute("mid").Value : "",
                                                     R_RWYmid = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                     R_AhpMid = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("AhpUid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("AhpUid").Attribute("mid").Value : "",
                                                     R_RwyTxt = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("txtDesig")!=null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("txtDesig").Value : "",
                                                     txtRmk = aixm45_obj.Element("txtRmk")!= null ? aixm45_obj.Element("txtRmk").Value : "",
                                                     CodeCat = aixm45_obj.Element("codeCat")!=null ? aixm45_obj.Element("codeCat").Value : "",
                                                     R_DmeMid = aixm45_obj.Element("DmeUid").Attribute("mid") != null ? aixm45_obj.Element("DmeUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_codeIdTxt = aixm45_obj.Element("Ilz").Element("codeId") != null ? aixm45_obj.Element("Ilz").Element("codeId").Value : "",
                                                     IGP = aixm45_obj.Element("Igp")!=null ? new AIXM45_IGP{
                                                         ValSlope = aixm45_obj.Element("Igp").Element("valSlope") !=null ?   Double.Parse(aixm45_obj.Element("Igp").Element("valSlope").Value) : 0,
                                                         ValElev = aixm45_obj.Element("Igp").Element("valElev") != null? Double.Parse( aixm45_obj.Element("Igp").Element("valElev").Value) : 0,
                                                         UomDistVer = aixm45_obj.Element("Igp").Element("uomDistVer") != null? aixm45_obj.Element("Igp").Element("uomDistVer").Value : "",
                                                         ID =  aixm45_obj.Element("Igp").Element("codeId") != null? aixm45_obj.Element("Igp").Element("codeId").Value : "",
                                                         GeoLat = aixm45_obj.Element("Igp").Element("geoLat") != null? aixm45_obj.Element("Igp").Element("geoLat").Value : "",
                                                         GeoLong = aixm45_obj.Element("Igp").Element("geoLong") != null? aixm45_obj.Element("Igp").Element("geoLong").Value : "",
                                                         R_ILSMID = aixm45_obj.Element("IlsUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                         ValFreq = aixm45_obj.Element("Igp").Element("valFreq") != null?  Double.Parse(aixm45_obj.Element("Igp").Element("valFreq").Value) : 0,
                                                         UomFreq = aixm45_obj.Element("Igp").Element("uomFreq") != null? aixm45_obj.Element("Igp").Element("uomFreq").Value : "",
                                                         ValRdh = aixm45_obj.Element("Igp").Element("valRdh") != null ? Double.Parse(aixm45_obj.Element("Igp").Element("valRdh").Value) : 0,
                                                         UomRdh =  aixm45_obj.Element("Igp").Element("uomRdh") != null ? aixm45_obj.Element("Igp").Element("uomRdh").Value : "",
                                                         R_RdnMid = aixm45_obj.Element("IlsUid").Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Attribute("mid").Value : "",
                                                         R_RWYmid = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                         R_AhpMid = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("AhpUid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("AhpUid").Attribute("mid").Value : "",

                                                         
                                                     } : null,
                                                     ILZ = aixm45_obj.Element("Ilz")!=null ? new AIXM45_ILZ{
                                                         CodeId = aixm45_obj.Element("Ilz").Element("codeId") != null ? aixm45_obj.Element("Ilz").Element("codeId").Value : "",
                                                         ValFreq = aixm45_obj.Element("Ilz").Element("valFreq") != null?  Double.Parse(aixm45_obj.Element("Ilz").Element("valFreq").Value) : 0,
                                                         UomFreq = aixm45_obj.Element("Ilz").Element("uomFreq") != null? aixm45_obj.Element("Ilz").Element("uomFreq").Value : "",
                                                         ValMagBrg = aixm45_obj.Element("Ilz").Element("valMagBrg") != null ? Double.Parse(aixm45_obj.Element("Ilz").Element("valMagBrg").Value) : 0,
                                                         ValTrueBrg = aixm45_obj.Element("Ilz").Element("valTrueBrg") != null ? Double.Parse(aixm45_obj.Element("Ilz").Element("valTrueBrg").Value) : 0,
                                                         ValWidCourse = aixm45_obj.Element("Ilz").Element("valWidCourse") != null ? Double.Parse(aixm45_obj.Element("Ilz").Element("valWidCourse").Value) : 0,
                                                         GeoLat = aixm45_obj.Element("Ilz").Element("geoLat") != null ? aixm45_obj.Element("Ilz").Element("geoLat").Value : "",
                                                         GeoLong = aixm45_obj.Element("Ilz").Element("geoLong") != null ? aixm45_obj.Element("Ilz").Element("geoLong").Value : "",
                                                         ValElev = aixm45_obj.Element("Ilz").Element("valElev") != null ? Double.Parse(aixm45_obj.Element("Ilz").Element("valElev").Value) : 0,
                                                         UomDistVer = aixm45_obj.Element("Ilz").Element("uomDistVer") != null ? aixm45_obj.Element("Ilz").Element("uomDistVer").Value : "",
                                                         R_ILSMID =  aixm45_obj.Element("IlsUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                         R_RdnMid = aixm45_obj.Element("IlsUid").Element("RdnUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Attribute("mid").Value : "",
                                                         R_RWYmid = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Attribute("mid").Value : "",
                                                         R_AhpMid = aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("AhpUid") != null ? aixm45_obj.Element("IlsUid").Element("RdnUid").Element("RwyUid").Element("AhpUid").Attribute("mid").Value : "",

                                                     } : null,
                                                     
                                                     #endregion
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_IlsList);

                        #endregion

                        break;

                    case("D."):

                        #region

                        var aixm45_DmeList = from aixm45_obj in xdoc.Descendants("Dme")
                                             where aixm45_obj.Element("DmeUid") != null
                                             select
                                                 new AIXM45_DME
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("DmeUid").Attribute("mid") != null ? aixm45_obj.Element("DmeUid").Attribute("mid").Value : Guid.NewGuid().ToString(), //mid
                                                     R_VorMid = aixm45_obj.Element("VorUid") != null ? aixm45_obj.Element("VorUid").Attribute("mid").Value : "", //ссылка на VOR
                                                     R_codeId = aixm45_obj.Element("DmeUid").Element("codeId") != null? aixm45_obj.Element("DmeUid").Element("codeId").Value : "", //ссылка на VOR
                                                     R_geoLat = aixm45_obj.Element("DmeUid").Element("geoLat") != null ? aixm45_obj.Element("DmeUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("DmeUid").Element("geoLong") != null ? aixm45_obj.Element("DmeUid").Element("geoLong").Value : "",
                                                     TxtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     CodeType = aixm45_obj.Element("codeType") != null ? aixm45_obj.Element("codeType").Value : "",
                                                     CodeChannel = aixm45_obj.Element("codeChannel") != null ? aixm45_obj.Element("codeChannel").Value : "",
                                                     ValElev = aixm45_obj.Element("valElev") != null ? Double.Parse(aixm45_obj.Element("valElev").Value) : 0,
                                                     UomDistVer = aixm45_obj.Element("uomDistVer") != null ? aixm45_obj.Element("uomDistVer").Value : "",
                                                    
                                                     #endregion
                                                 };


                        AIXM45_ObjectsList.AddRange(aixm45_DmeList);

                        var aixm45_VorList = from aixm45_obj in xdoc.Descendants("Vor")
                                             where aixm45_obj.Element("VorUid") != null
                                             select
                                                 new AIXM45_VOR
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("VorUid").Attribute("mid") != null ? aixm45_obj.Element("VorUid").Attribute("mid").Value : Guid.NewGuid().ToString(), //mid
                                                     CodeType = aixm45_obj.Element("codeType") !=null ? aixm45_obj.Element("codeType").Value : "",
                                                     ValFreq = aixm45_obj.Element("valFreq") != null ? Double.Parse(aixm45_obj.Element("valFreq").Value) : 0,
                                                     UomFreq = aixm45_obj.Element("uomFreq") !=null ? aixm45_obj.Element("uomFreq").Value : "",
                                                     ValMagVar = aixm45_obj.Element("valMagVar") != null ? Double.Parse(aixm45_obj.Element("valMagVar").Value) : 0,
                                                     ValElev = aixm45_obj.Element("valElev") != null ? Double.Parse(aixm45_obj.Element("valElev").Value) : 0,
                                                     UomDistVer = aixm45_obj.Element("uomDistVer") != null ? aixm45_obj.Element("uomDistVer").Value : "",
                                                     R_geoLat = aixm45_obj.Element("VorUid").Element("geoLat") != null ? aixm45_obj.Element("VorUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("VorUid").Element("geoLong") != null ? aixm45_obj.Element("VorUid").Element("geoLong").Value : "",
                                                     R_codeId = aixm45_obj.Element("VorUid").Element("codeId") != null ? aixm45_obj.Element("VorUid").Element("codeId").Value : "",
                                                     TxtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     #endregion
                                                 };


                       AIXM45_ObjectsList.AddRange(aixm45_VorList);


                    #endregion

                        break;

                    case("DB"):
                    case("PN"):

                        #region 

                        
                        var aixm45_NdbList = from aixm45_obj in xdoc.Descendants("Ndb")
                                             where aixm45_obj.Element("NdbUid") != null
                                             select
                                                 new AIXM45_NDB
                                                 {
                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("NdbUid").Attribute("mid") != null ? aixm45_obj.Element("NdbUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_codeId = aixm45_obj.Element("NdbUid").Element("codeId") != null ? aixm45_obj.Element("NdbUid").Element("codeId").Value : "",
                                                     R_geoLat = aixm45_obj.Element("NdbUid").Element("geoLat") != null ? aixm45_obj.Element("NdbUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("NdbUid").Element("geoLong") != null ? aixm45_obj.Element("NdbUid").Element("geoLong").Value : "",
                                                     TxtName = aixm45_obj.Element("txtName") != null? aixm45_obj.Element("txtName").Value : "",
                                                     ValFreq = aixm45_obj.Element("valFreq") != null ? Double.Parse(aixm45_obj.Element("valFreq").Value) : 0,
                                                     UomFreq = aixm45_obj.Element("uomFreq") != null ? aixm45_obj.Element("uomFreq").Value : "",
                                                     CodeClass = aixm45_obj.Element("codeClass") != null ? aixm45_obj.Element("codeClass").Value : "",

                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_NdbList);


                        #endregion

                        break;

                    case ("PM"):

                        #region


                        var aixm45_mkrList = from aixm45_obj in xdoc.Descendants("Mkr")
                                             where aixm45_obj.Element("MkrUid") != null
                                             select
                                                 new AIXM45_Marker
                                                 {
                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("MkrUid").Attribute("mid") != null ? aixm45_obj.Element("MkrUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_codeId = aixm45_obj.Element("MkrUid").Element("codeId") != null ? aixm45_obj.Element("MkrUid").Element("codeId").Value : "",
                                                     R_geoLat = aixm45_obj.Element("MkrUid").Element("geoLat") != null ? aixm45_obj.Element("MkrUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("MkrUid").Element("geoLong") != null ? aixm45_obj.Element("MkrUid").Element("geoLong").Value : "",
                                                     TxtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     ValFreq = aixm45_obj.Element("valFreq") != null ? Double.Parse(aixm45_obj.Element("valFreq").Value) : 0,
                                                     UomFreq = aixm45_obj.Element("uomFreq") != null ? aixm45_obj.Element("uomFreq").Value : "",
                                                     CodeClass = aixm45_obj.Element("codeClass") != null ? aixm45_obj.Element("codeClass").Value : "",
                                                     CodePsnIls = aixm45_obj.Element("codePsnIls") != null ? aixm45_obj.Element("codePsnIls").Value : "",
                                                     ValAxisBrg = aixm45_obj.Element("valAxisBrg") != null ? Double.Parse(aixm45_obj.Element("valAxisBrg").Value) : 0,
                                                     R_NDBMID = aixm45_obj.Element("NdbUid")!=null && aixm45_obj.Element("NdbUid").Attribute("mid") != null ? aixm45_obj.Element("NdbUid").Attribute("mid").Value : "",
                                                     R_ILSMID = aixm45_obj.Element("IlsUid")!=null && aixm45_obj.Element("IlsUid").Attribute("mid") != null ? aixm45_obj.Element("IlsUid").Attribute("mid").Value : "",
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_mkrList);


                        #endregion

                        break;

                    case ("EA"):
                    case ("PC"):

                        #region 

                        
                        var aixm45_DpnList = from aixm45_obj in xdoc.Descendants("Dpn")
                                             where aixm45_obj.Element("DpnUid") != null
                                             select
                                                 new AIXM45_WayPoint
                                                 {
                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("DpnUid").Attribute("mid") != null ? aixm45_obj.Element("DpnUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_codeId = aixm45_obj.Element("DpnUid").Element("codeId") != null ? aixm45_obj.Element("DpnUid").Element("codeId").Value : "",
                                                     R_geoLat = aixm45_obj.Element("DpnUid").Element("geoLat") != null ? aixm45_obj.Element("DpnUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("DpnUid").Element("geoLong") != null ? aixm45_obj.Element("DpnUid").Element("geoLong").Value : "",
                                                     CodeType = aixm45_obj.Element("codeType") !=null ? aixm45_obj.Element("codeType").Value : "",
                                                     TxtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     R_Ahpmid = aixm45_obj.Element("AhpUidAssoc")!= null ? aixm45_obj.Element("AhpUidAssoc").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_DpnList);


                        #endregion

                        break;


                    case ("ER"):
                    case ("EU"):

                      #region 

                       

                        var aixm45_routeList = from aixm45_obj in xdoc.Descendants("Rte")
                                               where aixm45_obj.Element("RteUid") != null
                                               select
                                                   new AIXM45_Enrote
                                                   { 
                                                       #region 

                                                       ID = Guid.NewGuid().ToString(),
                                                       R_mid = aixm45_obj.Element("RteUid").Attribute("mid") != null ? aixm45_obj.Element("RteUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                       R_txtDesig = aixm45_obj.Element("RteUid").Element("txtDesig") != null ? aixm45_obj.Element("RteUid").Element("txtDesig").Value : "",
                                                       R_txtLocDesig = aixm45_obj.Element("RteUid").Element("txtLocDesig") != null ? aixm45_obj.Element("RteUid").Element("txtLocDesig").Value : "",
                                                       TxtRmk = aixm45_obj.Element("txtRmk") != null ? aixm45_obj.Element("txtRmk").Value : "",

                                                       #endregion
                                                   };



                        AIXM45_ObjectsList.AddRange(aixm45_routeList);


                        var aixm45_routeSegmentList = from aixm45_obj in xdoc.Descendants("Rsg")
                                                      where aixm45_obj.Element("RsgUid") != null
                                                      select
                                                          new AIXM45_RouteSegment
                                                          {
                                                              #region

                                                              ID = Guid.NewGuid().ToString(),
                                                              R_mid = aixm45_obj.Element("RsgUid").Attribute("mid") != null ? aixm45_obj.Element("RsgUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                              R_RteMid = aixm45_obj.Element("RsgUid").Element("RteUid") != null ? aixm45_obj.Element("RsgUid").Element("RteUid").Attribute("mid").Value :  Guid.NewGuid().ToString(),
                                                              R_SignificantPointStaMid = aixm45_obj.Element("RsgUid").Element("VorUidSta") != null ? aixm45_obj.Element("RsgUid").Element("VorUidSta").Attribute("mid").Value :
                                                                                      aixm45_obj.Element("RsgUid").Element("DpnUidSta") != null ? aixm45_obj.Element("RsgUid").Element("DpnUidSta").Attribute("mid").Value :
                                                                                      aixm45_obj.Element("RsgUid").Element("TcnUidSta") != null ? aixm45_obj.Element("RsgUid").Element("TcnUidSta").Attribute("mid").Value :
                                                                                      aixm45_obj.Element("RsgUid").Element("NdbUidSta") != null ? aixm45_obj.Element("RsgUid").Element("NdbUidSta").Attribute("mid").Value :
                                                                                      aixm45_obj.Element("RsgUid").Element("DmeUidSta") != null ? aixm45_obj.Element("RsgUid").Element("DmeUidSta").Attribute("mid").Value :
                                                                                      aixm45_obj.Element("RsgUid").Element("MkrUidSta") != null ? aixm45_obj.Element("RsgUid").Element("MkrUidSta").Attribute("mid").Value : "",
                                                              R_SignificantPointStacode_Id = aixm45_obj.Element("RsgUid").Element("VorUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("VorUidSta").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DpnUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("DpnUidSta").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("TcnUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("TcnUidSta").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("NdbUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("NdbUidSta").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DmeUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("DmeUidSta").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("MkrUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("MkrUidSta").Element("codeId").Value : "",
                                                              R_SignificantPointSta_LAT = aixm45_obj.Element("RsgUid").Element("VorUidSta") != null ? aixm45_obj.Element("RsgUid").Element("VorUidSta").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DpnUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("DpnUidSta").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("TcnUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("TcnUidSta").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("NdbUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("NdbUidSta").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DmeUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("DmeUidSta").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("MkrUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("MkrUidSta").Element("geoLat").Value : "",
                                                              R_SignificantPointSta_LONG = aixm45_obj.Element("RsgUid").Element("VorUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("VorUidSta").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("DpnUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("DpnUidSta").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("TcnUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("TcnUidSta").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("NdbUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("NdbUidSta").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("DmeUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("DmeUidSta").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("MkrUidSta")!= null ? aixm45_obj.Element("RsgUid").Element("MkrUidSta").Element("geoLong").Value : "",
                                                              R_SignificantPointSta_PointChoice = aixm45_obj.Element("RsgUid").Element("VorUidSta") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                          aixm45_obj.Element("RsgUid").Element("DpnUidSta") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint :
                                                                                          aixm45_obj.Element("RsgUid").Element("TcnUidSta") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                          aixm45_obj.Element("RsgUid").Element("NdbUidSta") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.Navaid :
                                                                                          aixm45_obj.Element("RsgUid").Element("DmeUidSta") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                          aixm45_obj.Element("RsgUid").Element("MkrUidSta") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.OTHER : AIXM45_RouteSegment.AIXM45_PointChoice.NONE,
                                                              R_SignificantPointEndMid = aixm45_obj.Element("RsgUid").Element("VorUidEnd") != null ? aixm45_obj.Element("RsgUid").Element("VorUidEnd").Attribute("mid").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DpnUidEnd") != null ? aixm45_obj.Element("RsgUid").Element("DpnUidEnd").Attribute("mid").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("TcnUidEnd") != null ? aixm45_obj.Element("RsgUid").Element("TcnUidEnd").Attribute("mid").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("NdbUidEnd") != null ? aixm45_obj.Element("RsgUid").Element("NdbUidEnd").Attribute("mid").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DmeUidEnd") != null ? aixm45_obj.Element("RsgUid").Element("DmeUidEnd").Attribute("mid").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("MkrUidEnd") != null ? aixm45_obj.Element("RsgUid").Element("MkrUidEnd").Attribute("mid").Value : "",
                                                              R_SignificantPointEndcode_Id = aixm45_obj.Element("RsgUid").Element("VorUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("VorUidEnd").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DpnUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("DpnUidEnd").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("TcnUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("TcnUidEnd").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("NdbUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("NdbUidEnd").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DmeUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("DmeUidEnd").Element("codeId").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("MkrUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("MkrUidEnd").Element("codeId").Value : "",
                                                              R_SignificantPointEnd_LAT = aixm45_obj.Element("RsgUid").Element("VorUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("VorUidEnd").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DpnUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("DpnUidEnd").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("TcnUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("TcnUidEnd").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("NdbUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("NdbUidEnd").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("DmeUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("DmeUidEnd").Element("geoLat").Value :
                                                                                        aixm45_obj.Element("RsgUid").Element("MkrUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("MkrUidEnd").Element("geoLat").Value : "",
                                                              R_SignificantPointEnd_LONG = aixm45_obj.Element("RsgUid").Element("VorUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("VorUidEnd").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("DpnUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("DpnUidEnd").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("TcnUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("TcnUidEnd").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("NdbUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("NdbUidEnd").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("DmeUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("DmeUidEnd").Element("geoLong").Value :
                                                                                          aixm45_obj.Element("RsgUid").Element("MkrUidEnd")!= null ? aixm45_obj.Element("RsgUid").Element("MkrUidEnd").Element("geoLong").Value : "",
                                                              R_SignificantPointEnd_PointChoice = aixm45_obj.Element("RsgUid").Element("VorUidEnd") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                         aixm45_obj.Element("RsgUid").Element("DpnUidEnd") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint :
                                                                                         aixm45_obj.Element("RsgUid").Element("TcnUidEnd") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                         aixm45_obj.Element("RsgUid").Element("NdbUidEnd") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.Navaid :
                                                                                         aixm45_obj.Element("RsgUid").Element("DmeUidEnd") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                         aixm45_obj.Element("RsgUid").Element("MkrUidEnd") != null ? AIXM45_RouteSegment.AIXM45_PointChoice.OTHER : AIXM45_RouteSegment.AIXM45_PointChoice.NONE,
                                                              CodeCiv = aixm45_obj.Element("codeCiv") != null ? aixm45_obj.Element("codeCiv").Value : "",
                                                              CodeClassAcft = aixm45_obj.Element("codeClassAcft") != null ? aixm45_obj.Element("codeClassAcft").Value : "",
                                                              CodeDistVerLower = aixm45_obj.Element("codeDistVerLower") != null ? aixm45_obj.Element("codeDistVerLower").Value : "",
                                                              CodeDistVerLowerOvrde = aixm45_obj.Element("codeDistVerLowerOvrde") != null ? aixm45_obj.Element("codeDistVerLowerOvrde").Value : "",
                                                              CodeDistVerMnm = aixm45_obj.Element("codeDistVerMnm") != null ? aixm45_obj.Element("codeDistVerMnm").Value : "",
                                                              CodeDistVerUpper = aixm45_obj.Element("codeDistVerUpper") != null ? aixm45_obj.Element("codeDistVerUpper").Value : "",
                                                              CodeIntl = aixm45_obj.Element("codeIntl") != null ? aixm45_obj.Element("codeIntl").Value : "",
                                                              CodeLvl = aixm45_obj.Element("codeLvl") != null ? aixm45_obj.Element("codeLvl").Value : "",
                                                              CodeRepAtcEnd = aixm45_obj.Element("codeRepAtcEnd") != null ? aixm45_obj.Element("codeRepAtcEnd").Value : "",
                                                              CodeRepAtcStart = aixm45_obj.Element("codeRepAtcStart") != null ? aixm45_obj.Element("codeRepAtcStart").Value : "",
                                                              CodeRnp = aixm45_obj.Element("codeRnp") != null ? aixm45_obj.Element("codeRnp").Value : "",
                                                              CodeRvsmEnd = aixm45_obj.Element("codeRvsmEnd") != null ? aixm45_obj.Element("codeRvsmEnd").Value : "",
                                                              CodeRvsmStart = aixm45_obj.Element("codeRvsmStart") != null ? aixm45_obj.Element("codeRvsmStart").Value : "",
                                                              CodeType = aixm45_obj.Element("codeType") != null ? aixm45_obj.Element("codeType").Value : "",
                                                              CodeTypeFltRule = aixm45_obj.Element("codeTypeFltRule") != null ? aixm45_obj.Element("codeTypeFltRule").Value : "",
                                                              CodeTypePath = aixm45_obj.Element("codeTypePath") != null ? aixm45_obj.Element("codeTypePath").Value : "",
                                                              TxtRmk = aixm45_obj.Element("txtRmk") != null ? aixm45_obj.Element("txtRmk").Value : "",
                                                              UomDist = aixm45_obj.Element("uomDist") != null ? aixm45_obj.Element("uomDist").Value : "",
                                                              UomDistVerLower = aixm45_obj.Element("uomDistVerLower") != null ? aixm45_obj.Element("uomDistVerLower").Value : "",
                                                              UomDistVerLowerOvrde = aixm45_obj.Element("uomDistVerLowerOvrde") != null ? aixm45_obj.Element("uomDistVerLowerOvrde").Value : "",
                                                              UomDistVerMnm = aixm45_obj.Element("uomDistVerMnm") != null ? aixm45_obj.Element("uomDistVerMnm").Value : "",
                                                              UomDistVerUpper = aixm45_obj.Element("uomDistVerUpper") != null ? aixm45_obj.Element("uomDistVerUpper").Value : "",
                                                              UomWid = aixm45_obj.Element("uomWid") != null ? aixm45_obj.Element("uomWid").Value : "",
                                                              ValCopDist = aixm45_obj.Element("valCopDist") != null ? Double.Parse(aixm45_obj.Element("valCopDist").Value) : 0,
                                                              ValDistVerLower = aixm45_obj.Element("valDistVerLower") != null ? Double.Parse(aixm45_obj.Element("valDistVerLower").Value) : 0,
                                                              ValDistVerLowerOvrde = aixm45_obj.Element("valDistVerLowerOvrde") != null ? Double.Parse(aixm45_obj.Element("valDistVerLowerOvrde").Value) : 0,
                                                              ValDistVerMnm = aixm45_obj.Element("valDistVerMnm") != null ? Double.Parse(aixm45_obj.Element("valDistVerMnm").Value) : 0,
                                                              ValDistVerUpper = aixm45_obj.Element("valDistVerUpper") != null ? Double.Parse(aixm45_obj.Element("valDistVerUpper").Value) : 0,
                                                              ValLen = aixm45_obj.Element("valLen") != null ? Double.Parse(aixm45_obj.Element("valLen").Value) : 0,
                                                              ValMagTrack = aixm45_obj.Element("valMagTrack") != null ? Double.Parse(aixm45_obj.Element("valMagTrack").Value) : 0,
                                                              ValReversMagTrack = aixm45_obj.Element("valReversMagTrack") != null ? Double.Parse(aixm45_obj.Element("valReversMagTrack").Value) : 0,
                                                              ValReversTrueTrack = aixm45_obj.Element("valReversTrueTrack") != null ? Double.Parse(aixm45_obj.Element("valReversTrueTrack").Value) : 0,
                                                              ValTrueTrack = aixm45_obj.Element("valTrueTrack") != null ? Double.Parse(aixm45_obj.Element("valTrueTrack").Value) : 0,
                                                              ValWid = aixm45_obj.Element("valWid") != null ? Double.Parse(aixm45_obj.Element("valWid").Value) : 0,


                                                              #endregion
                                                          };

                        AIXM45_ObjectsList.AddRange(aixm45_routeSegmentList);



                      #endregion

                        break;


                    case ("UR"):
                    case ("UF"):
                    case ("UC"):

                        #region

                        var aixm45_ArspsList = from aixm45_obj in xdoc.Descendants("Ase")
                                             where aixm45_obj.Element("AseUid") != null && aixm45_obj.Element("AseUid").Element("codeId").Value.StartsWith(Area.FirstLetter)
                                             select
                                                 new AIXM45_Airspace
                                                 {
                                                     #region
                                                     
                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("AseUid").Attribute("mid") != null ? aixm45_obj.Element("AseUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_codeId = aixm45_obj.Element("AseUid").Element("codeId") != null ? aixm45_obj.Element("AseUid").Element("codeId").Value : "",
                                                     R_codeType = aixm45_obj.Element("AseUid").Element("codeType") != null ? aixm45_obj.Element("AseUid").Element("codeType").Value : "",
                                                     TxtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     TxtLocalType = aixm45_obj.Element("txtLocalType") != null ? aixm45_obj.Element("txtLocalType").Value : "",
                                                     TxtRmk = aixm45_obj.Element("txtRmk") != null ? aixm45_obj.Element("txtRmk").Value : "",
                                                     UomDistVerLower = aixm45_obj.Element("uomDistVerLower") != null ? aixm45_obj.Element("uomDistVerLower").Value : "",
                                                     UomDistVerMax = aixm45_obj.Element("uomDistVerMax") != null ? aixm45_obj.Element("uomDistVerMax").Value : "",
                                                     UomDistVerMnm = aixm45_obj.Element("uomDistVerMnm") != null ? aixm45_obj.Element("uomDistVerMnm").Value : "",
                                                     UomDistVerUpper = aixm45_obj.Element("uomDistVerUpper") != null ? aixm45_obj.Element("uomDistVerUpper").Value : "",
                                                     CodeActivity = aixm45_obj.Element("codeActivity") != null ? aixm45_obj.Element("codeActivity").Value : "",
                                                     CodeClass = aixm45_obj.Element("codeClass") != null ? aixm45_obj.Element("codeClass").Value : "",
                                                     CodeDistVerLower = aixm45_obj.Element("codeDistVerLower") != null ? aixm45_obj.Element("codeDistVerLower").Value : "",
                                                     CodeDistVerMax = aixm45_obj.Element("codeDistVerMax") != null ? aixm45_obj.Element("codeDistVerMax").Value : "",
                                                     CodeDistVerMnm = aixm45_obj.Element("codeDistVerMnm") != null ? aixm45_obj.Element("codeDistVerMnm").Value : "",
                                                     CodeDistVerUpper = aixm45_obj.Element("codeDistVerUpper") != null ? aixm45_obj.Element("codeDistVerUpper").Value : "",
                                                     CodeLocInd = aixm45_obj.Element("codeLocInd") != null ? aixm45_obj.Element("codeLocInd").Value : "",
                                                     CodeMil = aixm45_obj.Element("codeMil") != null ? aixm45_obj.Element("codeMil").Value : "",
                                                     ValDistVerLower = aixm45_obj.Element("valDistVerLower") != null ? Double.Parse(aixm45_obj.Element("valDistVerLower").Value) : 0,
                                                     ValDistVerMax = aixm45_obj.Element("valDistVerMax") != null ? Double.Parse(aixm45_obj.Element("valDistVerMax").Value) : 0,
                                                     ValDistVerMnm = aixm45_obj.Element("valDistVerMnm") != null ? Double.Parse(aixm45_obj.Element("valDistVerMnm").Value) : 0,
                                                     ValDistVerUpper = aixm45_obj.Element("valDistVerUpper") != null ? Double.Parse(aixm45_obj.Element("valDistVerUpper").Value) : 0,
                                                     ValLowerLimit = aixm45_obj.Element("valLowerLimit") != null ? Double.Parse(aixm45_obj.Element("valLowerLimit").Value) : 0,
                                                     Border = aixm45_obj.Element("AseUid").Attribute("mid") != null ?  GetBorder(aixm45_obj.Element("AseUid").Attribute("mid").Value) : null,

                                                     #endregion

                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_ArspsList);

                    
                    #endregion

                        break;

                    case("OB"):

                        var aixm45_ObsList = from aixm45_obj in xdoc.Descendants("Obs")
                                             where aixm45_obj.Element("ObsUid") != null
                                             select
                                                 new AIXM45_Obstacles
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = aixm45_obj.Element("ObsUid").Attribute("mid") != null ? aixm45_obj.Element("ObsUid").Attribute("mid").Value : Guid.NewGuid().ToString(),
                                                     R_geoLat = aixm45_obj.Element("ObsUid").Element("geoLat") != null ? aixm45_obj.Element("ObsUid").Element("geoLat").Value : "",
                                                     R_geoLong = aixm45_obj.Element("ObsUid").Element("geoLong") != null ? aixm45_obj.Element("ObsUid").Element("geoLong").Value : "",
                                                     valElev = aixm45_obj.Element("valElev") != null ? Double.Parse(aixm45_obj.Element("valElev").Value) : 0,
                                                     valElevAccuracy = aixm45_obj.Element("valElevAccuracy") != null ? Double.Parse(aixm45_obj.Element("valElevAccuracy").Value) : 0,
                                                     valGeoAccuracy = aixm45_obj.Element("valGeoAccuracy") != null ? Double.Parse(aixm45_obj.Element("valGeoAccuracy").Value) : 0,
                                                     valGeoidUndulation = aixm45_obj.Element("valGeoidUndulation") != null ? Double.Parse(aixm45_obj.Element("valGeoidUndulation").Value) : 0,
                                                     valHgt = aixm45_obj.Element("valHgt") != null ? Double.Parse(aixm45_obj.Element("valHgt").Value) : 0,
                                                     txtName = aixm45_obj.Element("txtName") != null ? aixm45_obj.Element("txtName").Value : "",
                                                     txtRmk = aixm45_obj.Element("txtRmk") != null ? aixm45_obj.Element("txtRmk").Value : "",
                                                     txtDescrLgt = aixm45_obj.Element("txtDescrLgt") != null ? aixm45_obj.Element("txtDescrLgt").Value : "",
                                                     txtDescrMarking = aixm45_obj.Element("txtDescrMarking") != null ? aixm45_obj.Element("txtDescrMarking").Value : "",
                                                     txtDescrType = aixm45_obj.Element("txtDescrType") != null ? aixm45_obj.Element("txtDescrType").Value : "",
                                                     txtVerDatum = aixm45_obj.Element("txtVerDatum") != null ? aixm45_obj.Element("txtVerDatum").Value : "",
                                                     codeGroup = aixm45_obj.Element("codeGroup") != null ? aixm45_obj.Element("codeGroup").Value.ToString().CompareTo("Y") == 0 : false,
                                                     codeLgt = aixm45_obj.Element("codeLgt") != null ? aixm45_obj.Element("codeLgt").Value.ToString().CompareTo("Y") == 0 : false,
                                                     uomDistVer = aixm45_obj.Element("uomDistVer") != null ? aixm45_obj.Element("uomDistVer").Value : "",
                                                     uomGeoAccuracy = aixm45_obj.Element("uomGeoAccuracy") != null ? aixm45_obj.Element("uomGeoAccuracy").Value : "",

                                                     #endregion
                                                 };

                        AIXM45_ObjectsList.AddRange(aixm45_ObsList);


                        break;


                    default:
                        break;
                }
            }



            this.Tag = AIXM45_ObjectsList;



        }

        private void DecodeMDB45_File()
        {

            List<string> ListOfSectionsType = new List<string>();

            #region выбор типов записей

            foreach (TreeNode Nd in treeView3.Nodes)
            {
                if ((Nd.Checked) && (Nd.Tag != null) && (Nd.Tag.ToString().Length > 0))
                {
                    string[] TAGS = Nd.Tag.ToString().Split(',');

                    foreach (string TAG in TAGS)
                        if (!ListOfSectionsType.Contains(TAG)) ListOfSectionsType.Add(TAG);
                }


                if (Nd.Nodes.Count > 0)
                {
                    foreach (TreeNode NdTags in Nd.Nodes)
                    {
                        if ((NdTags.Checked) && (NdTags.Tag != null) && (NdTags.Tag.ToString().Length > 0))
                        {
                            string[] TAGS = NdTags.Tag.ToString().Split(',');

                            foreach (string TAG in TAGS)
                                if (!ListOfSectionsType.Contains(TAG)) ListOfSectionsType.Add(TAG);
                        }
                    }
                }
            }

            #endregion


            IWorkspaceFactory2 workspaceFactory = (IWorkspaceFactory2)new AccessWorkspaceFactoryClass();
            IWorkspace Wksp = workspaceFactory.OpenFromFile(textBox1.Text,0);
            IFeatureWorkspace fWksp = (IFeatureWorkspace)Wksp;
            ITable table = null;
            ICursor pCursor = null;
            IRow pRow = null;

            IQueryFilter qfilter = new QueryFilterClass();
            qfilter.WhereClause = "OBJECTID > 0";


            foreach (string SectionsType in ListOfSectionsType)
            {
                switch (SectionsType)
                {
                    case ("PA"):

                        #region

                         table = fWksp.OpenTable("AD_HP");
                         pCursor = table.Search(qfilter, false);


                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_Airport
                                                 {
                                                     #region

                                                    ID =  Guid.NewGuid().ToString(),
                                                    R_MID= pRow.get_Value(pRow.Fields.FindField("R_MID"))!= null ? pRow.get_Value(pRow.Fields.FindField("R_MID")).ToString() : "",
                                                    TxtName= pRow.get_Value(pRow.Fields.FindField("txtName"))!= null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                    CodeIcao= pRow.get_Value(pRow.Fields.FindField("codeIcao"))!= null ? pRow.get_Value(pRow.Fields.FindField("codeIcao")).ToString(): "",
                                                    CodeIata= pRow.get_Value(pRow.Fields.FindField("codeIata"))!= null ? pRow.get_Value(pRow.Fields.FindField("codeIata")).ToString(): "",
                                                    CodeType= pRow.get_Value(pRow.Fields.FindField("codeType"))!= null ? pRow.get_Value(pRow.Fields.FindField("codeType")).ToString(): "",
                                                    CodeTypeMilOps= pRow.get_Value(pRow.Fields.FindField("codeTypeMilOps"))!= null ? pRow.get_Value(pRow.Fields.FindField("codeTypeMilOps")).ToString(): "",
                                                    TxtDescrRefPt= pRow.get_Value(pRow.Fields.FindField("txtDescrRefPt"))!= null ? pRow.get_Value(pRow.Fields.FindField("txtDescrRefPt")).ToString(): "",
                                                    GeoLat= pRow.get_Value(pRow.Fields.FindField("geoLat"))!= null ? pRow.get_Value(pRow.Fields.FindField("geoLat")).ToString(): "",
                                                    GeoLong= pRow.get_Value(pRow.Fields.FindField("geoLong"))!= null ? pRow.get_Value(pRow.Fields.FindField("geoLong")).ToString(): "",
                                                    ValGeoAccuracy= pRow.get_Value(pRow.Fields.FindField("valGeoAccuracy"))!= null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valGeoAccuracy")).ToString()) : 0,
                                                    UomGeoAccuracy= pRow.get_Value(pRow.Fields.FindField("uomGeoAccuracy"))!= null ? pRow.get_Value(pRow.Fields.FindField("uomGeoAccuracy")).ToString(): "",
                                                    ValElev= pRow.get_Value(pRow.Fields.FindField("valElev"))!= null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valElev")).ToString()) : 0,
                                                    ValElevAccuracy= pRow.get_Value(pRow.Fields.FindField("valElevAccuracy"))!= null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valElevAccuracy")).ToString()) : 0,
                                                    UomDistVer= pRow.get_Value(pRow.Fields.FindField("uomDistVer"))!= null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString(): "",
                                                    TxtNameCitySer= pRow.get_Value(pRow.Fields.FindField("txtNameCitySer"))!= null ? pRow.get_Value(pRow.Fields.FindField("txtNameCitySer")).ToString(): "",
                                                    ValMagVar= pRow.get_Value(pRow.Fields.FindField("valMagVar"))!= null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valMagVar")).ToString()): 0,
                                                    ValTransitionAlt= pRow.get_Value(pRow.Fields.FindField("valTransitionAlt"))!= null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valTransitionAlt")).ToString()) :0,
                                                    UomTransitionAlt= pRow.get_Value(pRow.Fields.FindField("uomTransitionAlt"))!= null ? pRow.get_Value(pRow.Fields.FindField("uomTransitionAlt")).ToString(): "",
                                                    R_codeId= pRow.get_Value(pRow.Fields.FindField("R_codeId"))!= null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                                    Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }

                        break;

                    case ("PG"):

                         table = fWksp.OpenTable("RWY");
                         pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_RWY
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_AHPmid = pRow.get_Value(pRow.Fields.FindField("R_AHPmid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_AHPmid")).ToString() : "",
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_txtDesig = pRow.get_Value(pRow.Fields.FindField("R_txtDesig")) != null ? pRow.get_Value(pRow.Fields.FindField("R_txtDesig")).ToString() : "",
                                                     ValLen = pRow.get_Value(pRow.Fields.FindField("valLen")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valLen")).ToString()) : 0,
                                                     ValWid = pRow.get_Value(pRow.Fields.FindField("valWid")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valWid")).ToString()) : 0,
                                                     UomDimRwy = pRow.get_Value(pRow.Fields.FindField("uomDimRwy")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDimRwy")).ToString() : "",
                                                     //codeComposition = pRow.get_Value(pRow.Fields.FindField("codeComposition")) != null ? pRow.get_Value(pRow.Fields.FindField("codeComposition")).ToString() : "",
                                                     //codeStrength = pRow.get_Value(pRow.Fields.FindField("codeStrength")) != null ? pRow.get_Value(pRow.Fields.FindField("codeStrength")).ToString() : "",
                                                     //txtDescrStrength = pRow.get_Value(pRow.Fields.FindField("txtDescrStrength")) != null ? pRow.get_Value(pRow.Fields.FindField("txtDescrStrength")).ToString() : "",
                                                     ValLenStrip = pRow.get_Value(pRow.Fields.FindField("valLenStrip")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valLenStrip")).ToString()) : 0,
                                                     ValWidStrip = pRow.get_Value(pRow.Fields.FindField("valWidStrip")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valWidStrip")).ToString()) : 0,
                                                     //valLenOffset = pRow.get_Value(pRow.Fields.FindField("valLenOffset")) != null ? pRow.get_Value(pRow.Fields.FindField("valLenOffset")).ToString() : "",
                                                     //valWidOffset = pRow.get_Value(pRow.Fields.FindField("valWidOffset")) != null ? pRow.get_Value(pRow.Fields.FindField("valWidOffset")).ToString() : "",
                                                     UomDimStrip = pRow.get_Value(pRow.Fields.FindField("uomDimStrip")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDimStrip")).ToString() : "",
                                                     //codeSts = pRow.get_Value(pRow.Fields.FindField("codeSts")) != null ? pRow.get_Value(pRow.Fields.FindField("codeSts")).ToString() : "",
                                                     //txtProfile = pRow.get_Value(pRow.Fields.FindField("txtProfile")) != null ? pRow.get_Value(pRow.Fields.FindField("txtProfile")).ToString() : "",
                                                     //txtMarking = pRow.get_Value(pRow.Fields.FindField("txtMarking")) != null ? pRow.get_Value(pRow.Fields.FindField("txtMarking")).ToString() : "",
                                                     //txtRmk = pRow.get_Value(pRow.Fields.FindField("txtRmk")) != null ? pRow.get_Value(pRow.Fields.FindField("txtRmk")).ToString() : "",
                                                     //codePreparation = pRow.get_Value(pRow.Fields.FindField("codePreparation")) != null ? pRow.get_Value(pRow.Fields.FindField("codePreparation")).ToString() : "",
                                                     //codeCondSfc = pRow.get_Value(pRow.Fields.FindField("codeCondSfc")) != null ? pRow.get_Value(pRow.Fields.FindField("codeCondSfc")).ToString() : "",
                                                     //valPcnClass = pRow.get_Value(pRow.Fields.FindField("valPcnClass")) != null ? pRow.get_Value(pRow.Fields.FindField("valPcnClass")).ToString() : "",
                                                     //codePcnPavementType = pRow.get_Value(pRow.Fields.FindField("codePcnPavementType")) != null ? pRow.get_Value(pRow.Fields.FindField("codePcnPavementType")).ToString() : "",
                                                     //codePcnPavementSubgrade = pRow.get_Value(pRow.Fields.FindField("codePcnPavementSubgrade")) != null ? pRow.get_Value(pRow.Fields.FindField("codePcnPavementSubgrade")).ToString() : "",
                                                     //codePcnMaxTirePressure = pRow.get_Value(pRow.Fields.FindField("codePcnMaxTirePressure")) != null ? pRow.get_Value(pRow.Fields.FindField("codePcnMaxTirePressure")).ToString() : "",
                                                     //valPcnMaxTirePressure = pRow.get_Value(pRow.Fields.FindField("valPcnMaxTirePressure")) != null ? pRow.get_Value(pRow.Fields.FindField("valPcnMaxTirePressure")).ToString() : "",
                                                     //codePcnEvalMethod = pRow.get_Value(pRow.Fields.FindField("codePcnEvalMethod")) != null ? pRow.get_Value(pRow.Fields.FindField("codePcnEvalMethod")).ToString() : "",
                                                     //txtPcnNote = pRow.get_Value(pRow.Fields.FindField("txtPcnNote")) != null ? pRow.get_Value(pRow.Fields.FindField("txtPcnNote")).ToString() : "",
                                                     //valLcnClass = pRow.get_Value(pRow.Fields.FindField("valLcnClass")) != null ? pRow.get_Value(pRow.Fields.FindField("valLcnClass")).ToString() : "",
                                                     //valSiwlWeight = pRow.get_Value(pRow.Fields.FindField("valSiwlWeight")) != null ? pRow.get_Value(pRow.Fields.FindField("valSiwlWeight")).ToString() : "",
                                                     //uomSiwlWeight = pRow.get_Value(pRow.Fields.FindField("uomSiwlWeight")) != null ? pRow.get_Value(pRow.Fields.FindField("uomSiwlWeight")).ToString() : "",
                                                     //valSiwlTirePressure = pRow.get_Value(pRow.Fields.FindField("valSiwlTirePressure")) != null ? pRow.get_Value(pRow.Fields.FindField("valSiwlTirePressure")).ToString() : "",
                                                     //uomSiwlTirePressure = pRow.get_Value(pRow.Fields.FindField("uomSiwlTirePressure")) != null ? pRow.get_Value(pRow.Fields.FindField("uomSiwlTirePressure")).ToString() : "",
                                                     //valAuwWeight = pRow.get_Value(pRow.Fields.FindField("valAuwWeight")) != null ? pRow.get_Value(pRow.Fields.FindField("valAuwWeight")).ToString() : "",
                                                     //uomAuwWeight = pRow.get_Value(pRow.Fields.FindField("uomAuwWeight")) != null ? pRow.get_Value(pRow.Fields.FindField("uomAuwWeight")).ToString() : "",


                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }

                        table = fWksp.OpenTable("RwyDirection");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_RDN
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_RWYmid = pRow.get_Value(pRow.Fields.FindField("R_RWYmid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RWYmid")).ToString() : "",
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     GeoLat = pRow.get_Value(pRow.Fields.FindField("geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("geoLat")).ToString() : "",
                                                     GeoLong = pRow.get_Value(pRow.Fields.FindField("geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("geoLong")).ToString() : "",
                                                     R_txtDesig = pRow.get_Value(pRow.Fields.FindField("R_txtDesig")) != null ? pRow.get_Value(pRow.Fields.FindField("R_txtDesig")).ToString() : "",
                                                     ValTrueBrg = pRow.get_Value(pRow.Fields.FindField("valTrueBrg")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valTrueBrg")).ToString()) : 0,
                                                     ValMagBrg = pRow.get_Value(pRow.Fields.FindField("valMagBrg")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valMagBrg")).ToString()) : 0,
                                                     ValElevTdz = pRow.get_Value(pRow.Fields.FindField("valElevTdz")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valElevTdz")).ToString()) : 0,
                                                     UomElevTdz = pRow.get_Value(pRow.Fields.FindField("uomElevTdz")) != null ? pRow.get_Value(pRow.Fields.FindField("uomElevTdz")).ToString() : "",
                                                     R_AhpMid = pRow.get_Value(pRow.Fields.FindField("R_AhpMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_AhpMid")).ToString() : "",
                                                     R_RdnElev = pRow.get_Value(pRow.Fields.FindField("R_RdnElev")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("R_RdnElev")).ToString()) : 0,
                                                     R_RdnElevUom = pRow.get_Value(pRow.Fields.FindField("R_RdnElevUom")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnElevUom")).ToString() : "",
                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }

                        table = fWksp.OpenTable("RwyClinePoint");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_RCP
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_RWYMID = pRow.get_Value(pRow.Fields.FindField("R_RWYMID")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RWYMID")).ToString() : "",
                                                     R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                                     R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                                     valElev = pRow.get_Value(pRow.Fields.FindField("valElev")) != null ?ToDouble( pRow.get_Value(pRow.Fields.FindField("valElev")).ToString() ): 0,
                                                     uomDistVer = pRow.get_Value(pRow.Fields.FindField("uomDistVer")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString() : "",
                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }

                        table = fWksp.OpenTable("RunwayDirectionDeclaredDistance");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_RDN_DECLARED_DISTANCE
                                                 {
                                                     #region

                                                     ID =Guid.NewGuid().ToString(),

                                                     valDist = pRow.get_Value(pRow.Fields.FindField("valDist")) != null ?ToDouble( pRow.get_Value(pRow.Fields.FindField("valDist")).ToString()) : 0,
                                                     uomDist = pRow.get_Value(pRow.Fields.FindField("uomDist")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDist")).ToString() : "",
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_RdnMid = pRow.get_Value(pRow.Fields.FindField("R_RdnMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnMid")).ToString() : "",
                                                     R_codeType = pRow.get_Value(pRow.Fields.FindField("R_codeType")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeType")).ToString() : "",
                                                     R_RWYMid = pRow.get_Value(pRow.Fields.FindField("R_RWYMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RWYMid")).ToString() : "",
                                                     R_RdnTxtDesid = pRow.get_Value(pRow.Fields.FindField("R_RdnTxtDesid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnTxtDesid")).ToString() : "",

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }

                        table = fWksp.OpenTable("Stopway");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_RDN_SWY
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),

                                                     valLen = pRow.get_Value(pRow.Fields.FindField("valLen")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valLen")).ToString()): 0,
                                                     valWid = pRow.get_Value(pRow.Fields.FindField("valWid")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valWid")).ToString()) : 0,
                                                     uomDim = pRow.get_Value(pRow.Fields.FindField("uomDim")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDim")).ToString() : "",
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_RdnMid = pRow.get_Value(pRow.Fields.FindField("R_RdnMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnMid")).ToString() : "",
                                                     R_RWYMid = pRow.get_Value(pRow.Fields.FindField("R_RWYMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RWYMid")).ToString() : "",
                                                     R_RdnTxtDesid = pRow.get_Value(pRow.Fields.FindField("R_RdnTxtDesid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnTxtDesid")).ToString() : "",


                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }

                        #endregion

                        break;

                    case ("PI"):

                        #region

                        table = fWksp.OpenTable("ILS");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_ILS
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     CodeCat = pRow.get_Value(pRow.Fields.FindField("codeCat")) != null ? pRow.get_Value(pRow.Fields.FindField("codeCat")).ToString() : "",
                                                     txtRmk = pRow.get_Value(pRow.Fields.FindField("txtRmk")) != null ? pRow.get_Value(pRow.Fields.FindField("txtRmk")).ToString() : "",
                                                     R_RdnMid = pRow.get_Value(pRow.Fields.FindField("R_RdnMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnMid")).ToString() : "",
                                                     R_DmeMid = pRow.get_Value(pRow.Fields.FindField("R_DmeMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_DmeMid")).ToString() : "",
                                                     R_RdnTxt = pRow.get_Value(pRow.Fields.FindField("R_RdnTxt")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RdnTxt")).ToString() : "",
                                                     R_RwyTxt = pRow.get_Value(pRow.Fields.FindField("R_RwyTxt")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RwyTxt")).ToString() : "",
                                                     R_codeIdTxt = pRow.get_Value(pRow.Fields.FindField("R_codeIdTxt")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeIdTxt")).ToString() : "",
                                                     IGP = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? GetIGP(fWksp, pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString()) : null,
                                                     ILZ = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? GetILZ(fWksp, pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString()) : null,
                                                     R_AhpMid = "",

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;

                    case ("D."):

                        #region

                        table = fWksp.OpenTable("DME");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_DME
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_codeId = pRow.get_Value(pRow.Fields.FindField("R_codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                                     R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                                     R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                                     R_VorMid = pRow.get_Value(pRow.Fields.FindField("R_VorMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_VorMid")).ToString() : "",
                                                     TxtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                     CodeType = pRow.get_Value(pRow.Fields.FindField("codeType")) != null ? pRow.get_Value(pRow.Fields.FindField("codeType")).ToString() : "",
                                                     CodeChannel = pRow.get_Value(pRow.Fields.FindField("codeChannel")) != null ? pRow.get_Value(pRow.Fields.FindField("codeChannel")).ToString() : "",
                                                     ValFreq = pRow.get_Value(pRow.Fields.FindField("valGhostFreq")) != null ?  pRow.get_Value(pRow.Fields.FindField("valGhostFreq")).ToString() :"",
                                                     UomFreq = pRow.get_Value(pRow.Fields.FindField("uomGhostFreq")) != null ? pRow.get_Value(pRow.Fields.FindField("uomGhostFreq")).ToString() : "",
                                                     ValElev = pRow.get_Value(pRow.Fields.FindField("valElev")) != null ?ToDouble( pRow.get_Value(pRow.Fields.FindField("valElev")).ToString()) : 0,
                                                     UomDistVer = pRow.get_Value(pRow.Fields.FindField("uomDistVer")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString() : "",

                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        table = fWksp.OpenTable("VOR");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_VOR
                                                 {
                                                     #region

                                                     ID =  Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_codeId = pRow.get_Value(pRow.Fields.FindField("R_codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                                     R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                                     R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                                     TxtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                     CodeType = pRow.get_Value(pRow.Fields.FindField("codeType")) != null ? pRow.get_Value(pRow.Fields.FindField("codeType")).ToString() : "",
                                                     ValFreq = pRow.get_Value(pRow.Fields.FindField("valFreq")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valFreq")).ToString()) : 0,
                                                     UomFreq = pRow.get_Value(pRow.Fields.FindField("uomFreq")) != null ? pRow.get_Value(pRow.Fields.FindField("uomFreq")).ToString() : "",
                                                     ValMagVar = pRow.get_Value(pRow.Fields.FindField("valMagVar")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valMagVar")).ToString()) : 0,
                                                     ValElev = pRow.get_Value(pRow.Fields.FindField("valElev")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valElev")).ToString()) : 0,
                                                     UomDistVer = pRow.get_Value(pRow.Fields.FindField("uomDistVer")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString() : "",

                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }



                        #endregion

                        break;

                    case ("DB"):
                    case ("PN"):

                        #region

                        table = fWksp.OpenTable("NDB");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_NDB
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_codeId = pRow.get_Value(pRow.Fields.FindField("R_codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                                     R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                                     R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                                     TxtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                     ValFreq = pRow.get_Value(pRow.Fields.FindField("valFreq")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valFreq")).ToString()) : 0,
                                                     UomFreq = pRow.get_Value(pRow.Fields.FindField("uomFreq")) != null ? pRow.get_Value(pRow.Fields.FindField("uomFreq")).ToString() : "",
                                                     ValMagVar = pRow.get_Value(pRow.Fields.FindField("valMagVar")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valMagVar")).ToString()) : 0,
                                                     CodeClass = pRow.get_Value(pRow.Fields.FindField("codeClass")) != null ? pRow.get_Value(pRow.Fields.FindField("codeClass")).ToString() : "",
                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;

                    case ("PM"):

                        #region

                        table = fWksp.OpenTable("Marker");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_Marker
                            {
                                #region

                                ID = Guid.NewGuid().ToString(),
                                R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                R_codeId = pRow.get_Value(pRow.Fields.FindField("R_codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                TxtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                ValFreq = pRow.get_Value(pRow.Fields.FindField("valFreq")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valFreq")).ToString()) : 0,
                                UomFreq = pRow.get_Value(pRow.Fields.FindField("uomFreq")) != null ? pRow.get_Value(pRow.Fields.FindField("uomFreq")).ToString() : "",
                                ValAxisBrg = pRow.get_Value(pRow.Fields.FindField("valAxisBrg")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valAxisBrg")).ToString()) : 0,
                                CodeClass = pRow.get_Value(pRow.Fields.FindField("codeClass")) != null ? pRow.get_Value(pRow.Fields.FindField("codeClass")).ToString() : "",
                                CodePsnIls = pRow.get_Value(pRow.Fields.FindField("codePsnIls")) != null ? pRow.get_Value(pRow.Fields.FindField("codePsnIls")).ToString() : "",
                                R_NDBMID = pRow.get_Value(pRow.Fields.FindField("R_NDBMID")) != null ? pRow.get_Value(pRow.Fields.FindField("R_NDBMID")).ToString() : "",
                                Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                #endregion
                            });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;

                    case ("EA"):
                    case ("PC"):

                        #region

                        table = fWksp.OpenTable("DesignatedPoint");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_WayPoint
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_codeId = pRow.get_Value(pRow.Fields.FindField("R_codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                                     R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                                     R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                                     CodeType = pRow.get_Value(pRow.Fields.FindField("codeType")) != null ? pRow.get_Value(pRow.Fields.FindField("codeType")).ToString() : "",
                                                     TxtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;


                    case ("ER"):
                    case ("EU"):

                        #region

                        table = fWksp.OpenTable("EnrouteRoute");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_Enrote
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_txtDesig = pRow.get_Value(pRow.Fields.FindField("R_txtDesig")) != null ? pRow.get_Value(pRow.Fields.FindField("R_txtDesig")).ToString() : "",
                                                     R_txtLocDesig = pRow.get_Value(pRow.Fields.FindField("R_txtLocDesig")) != null ? pRow.get_Value(pRow.Fields.FindField("R_txtLocDesig")).ToString() : "",
                                                     TxtRmk = pRow.get_Value(pRow.Fields.FindField("txtRmk")) != null ? pRow.get_Value(pRow.Fields.FindField("txtRmk")).ToString() : "",


                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }



                        table = fWksp.OpenTable("RouteSegment");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_RouteSegment
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_RsgMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RsgMid")).ToString() : "",
                                                     CodeType = pRow.get_Value(pRow.Fields.FindField("codeType")) != null ? pRow.get_Value(pRow.Fields.FindField("codeType")).ToString() : "",
                                                     CodeRnp = pRow.get_Value(pRow.Fields.FindField("codeRnp")) != null ? pRow.get_Value(pRow.Fields.FindField("codeRnp")).ToString() : "",
                                                     CodeLvl = pRow.get_Value(pRow.Fields.FindField("codeLvl")) != null ? pRow.get_Value(pRow.Fields.FindField("codeLvl")).ToString() : "",
                                                     CodeClassAcft = pRow.get_Value(pRow.Fields.FindField("codeClassAcft")) != null ? pRow.get_Value(pRow.Fields.FindField("codeClassAcft")).ToString() : "",
                                                     CodeIntl = pRow.get_Value(pRow.Fields.FindField("codeIntl")) != null ? pRow.get_Value(pRow.Fields.FindField("codeIntl")).ToString() : "",
                                                     CodeTypeFltRule = pRow.get_Value(pRow.Fields.FindField("codeTypeFltRule")) != null ? pRow.get_Value(pRow.Fields.FindField("codeTypeFltRule")).ToString() : "",
                                                     CodeCiv = pRow.get_Value(pRow.Fields.FindField("codeCiv")) != null ? pRow.get_Value(pRow.Fields.FindField("codeCiv")).ToString() : "",
                                                     ValDistVerUpper = pRow.get_Value(pRow.Fields.FindField("valDistVerUpper")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valDistVerUpper")).ToString()) : 0 ,
                                                     UomDistVerUpper = pRow.get_Value(pRow.Fields.FindField("uomDistVerUpper")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerUpper")).ToString() : "",
                                                     CodeDistVerUpper = pRow.get_Value(pRow.Fields.FindField("codeDistVerUpper")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerUpper")).ToString() : "",
                                                     ValDistVerLower = pRow.get_Value(pRow.Fields.FindField("valDistVerLower")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valDistVerLower")).ToString()) : 0,
                                                     UomDistVerLower = pRow.get_Value(pRow.Fields.FindField("uomDistVerLower")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerLower")).ToString() : "",
                                                     CodeDistVerLower = pRow.get_Value(pRow.Fields.FindField("codeDistVerLower")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerLower")).ToString() : "",
                                                     ValDistVerMnm = pRow.get_Value(pRow.Fields.FindField("valDistVerMnm")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valDistVerMnm")).ToString()) : 0,
                                                     UomDistVerMnm = pRow.get_Value(pRow.Fields.FindField("uomDistVerMnm")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerMnm")).ToString() : "",
                                                     CodeDistVerMnm = pRow.get_Value(pRow.Fields.FindField("codeDistVerMnm")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerMnm")).ToString() : "",
                                                     ValDistVerLowerOvrde = pRow.get_Value(pRow.Fields.FindField("valDistVerLowerOvrde")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valDistVerLowerOvrde")).ToString()): 0,
                                                     UomDistVerLowerOvrde = pRow.get_Value(pRow.Fields.FindField("uomDistVerLowerOvrde")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerLowerOvrde")).ToString() : "",
                                                     CodeDistVerLowerOvrde = pRow.get_Value(pRow.Fields.FindField("codeDistVerLowerOvrde")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerLowerOvrde")).ToString() : "",
                                                     ValWid = pRow.get_Value(pRow.Fields.FindField("valWid")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valWid")).ToString()) : 0,
                                                     UomWid = pRow.get_Value(pRow.Fields.FindField("uomWid")) != null ? pRow.get_Value(pRow.Fields.FindField("uomWid")).ToString() : "",
                                                     CodeRepAtcStart = pRow.get_Value(pRow.Fields.FindField("codeRepAtcStart")) != null ? pRow.get_Value(pRow.Fields.FindField("codeRepAtcStart")).ToString() : "",
                                                     CodeRepAtcEnd = pRow.get_Value(pRow.Fields.FindField("codeRepAtcEnd")) != null ? pRow.get_Value(pRow.Fields.FindField("codeRepAtcEnd")).ToString() : "",
                                                     CodeRvsmStart = pRow.get_Value(pRow.Fields.FindField("codeRvsmStart")) != null ? pRow.get_Value(pRow.Fields.FindField("codeRvsmStart")).ToString() : "",
                                                     CodeRvsmEnd = pRow.get_Value(pRow.Fields.FindField("codeRvsmEnd")) != null ? pRow.get_Value(pRow.Fields.FindField("codeRvsmEnd")).ToString() : "",
                                                     CodeTypePath = pRow.get_Value(pRow.Fields.FindField("codeTypePath")) != null ? pRow.get_Value(pRow.Fields.FindField("codeTypePath")).ToString() : "",
                                                     ValTrueTrack = pRow.get_Value(pRow.Fields.FindField("valTrueTrack")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valTrueTrack")).ToString()) : 0,
                                                     ValMagTrack = pRow.get_Value(pRow.Fields.FindField("valMagTrack")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valMagTrack")).ToString()): 0,
                                                     ValReversTrueTrack = pRow.get_Value(pRow.Fields.FindField("valReversTrueTrack")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valReversTrueTrack")).ToString()) : 0,
                                                     ValReversMagTrack = pRow.get_Value(pRow.Fields.FindField("valReversMagTrack")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valReversMagTrack")).ToString()): 0,
                                                     ValLen = pRow.get_Value(pRow.Fields.FindField("valLen")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valLen")).ToString()) : 0,
                                                     ValCopDist = pRow.get_Value(pRow.Fields.FindField("valCopDist")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valCopDist")).ToString()) : 0,
                                                     UomDist = pRow.get_Value(pRow.Fields.FindField("uomDist")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDist")).ToString() : "",
                                                     TxtRmk = pRow.get_Value(pRow.Fields.FindField("txtRmk")) != null ? pRow.get_Value(pRow.Fields.FindField("txtRmk")).ToString() : "",
                                                     R_RteMid = pRow.get_Value(pRow.Fields.FindField("R_RteMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_RteMid")).ToString() : "",
                                                     R_SignificantPointStaMid = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaMid")).ToString() : "",
                                                     R_SignificantPointStacode_Id = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStacode_Id")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStacode_Id")).ToString() : "",
                                                     R_SignificantPointSta_LAT = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStageo_LAT")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStageo_LAT")).ToString() : "",
                                                     R_SignificantPointSta_LONG = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStageo_LONG")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStageo_LONG")).ToString() : "",
                                                     R_SignificantPointEndMid = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndMid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndMid")).ToString() : "",
                                                     R_SignificantPointEndcode_Id = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndcode_Id")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndcode_Id")).ToString() : "",
                                                     R_SignificantPointEnd_LAT = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndgeo_LAT")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndgeo_LAT")).ToString() : "",
                                                     R_SignificantPointEnd_LONG = pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndcode_LONG")) != null ? pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndcode_LONG")).ToString() : "",
                                                     R_SignificantPointSta_PointChoice = (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")) != null &&   
                                                                                         pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")).ToString().CompareTo("VorUidSta")==0)? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid:
                                                                                         (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")) != null &&   
                                                                                         pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")).ToString().CompareTo("DpnUidSta")==0)? AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint :
                                                                                         (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")) != null &&
                                                                                         pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")).ToString().CompareTo("TcnUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                         (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")) != null &&
                                                                                         pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")).ToString().CompareTo("NdbUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.Navaid :
                                                                                         (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")) != null &&
                                                                                         pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")).ToString().CompareTo("DmeUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                         (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")) != null &&
                                                                                         pRow.get_Value(pRow.Fields.FindField("R_SignificantPointStaTYPE")).ToString().CompareTo("MkrUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.OTHER : AIXM45_RouteSegment.AIXM45_PointChoice.NONE,
                                                     R_SignificantPointEnd_PointChoice = (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")) != null &&
                                                                                          pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")).ToString().CompareTo("VorUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                          (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")) != null &&
                                                                                          pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")).ToString().CompareTo("DpnUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.DesignatedPoint :
                                                                                          (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")) != null &&
                                                                                          pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")).ToString().CompareTo("TcnUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                          (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")) != null &&
                                                                                          pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")).ToString().CompareTo("NdbUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.Navaid :
                                                                                          (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")) != null &&
                                                                                          pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")).ToString().CompareTo("DmeUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.VHFNavaid :
                                                                                          (pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")) != null &&
                                                                                          pRow.get_Value(pRow.Fields.FindField("R_SignificantPointEndTYPE")).ToString().CompareTo("MkrUidSta") == 0) ? AIXM45_RouteSegment.AIXM45_PointChoice.OTHER : AIXM45_RouteSegment.AIXM45_PointChoice.NONE,
                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;


                    case ("UR"):
                    case ("UF"):
                    case ("UC"):

                        #region

                        table = fWksp.OpenTable("Airspace");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_Airspace
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     TxtLocalType = pRow.get_Value(pRow.Fields.FindField("txtLocalType")) != null ? pRow.get_Value(pRow.Fields.FindField("txtLocalType")).ToString() : "",
                                                     TxtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                     CodeClass = pRow.get_Value(pRow.Fields.FindField("codeClass")) != null ? pRow.get_Value(pRow.Fields.FindField("codeClass")).ToString() : "",
                                                     CodeLocInd = pRow.get_Value(pRow.Fields.FindField("codeLocInd")) != null ? pRow.get_Value(pRow.Fields.FindField("codeLocInd")).ToString() : "",
                                                     CodeActivity = pRow.get_Value(pRow.Fields.FindField("codeActivity")) != null ? pRow.get_Value(pRow.Fields.FindField("codeActivity")).ToString() : "",
                                                     CodeMil = pRow.get_Value(pRow.Fields.FindField("codeMil")) != null ? pRow.get_Value(pRow.Fields.FindField("codeMil")).ToString() : "",
                                                     CodeDistVerUpper = pRow.get_Value(pRow.Fields.FindField("codeDistVerUpper")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerUpper")).ToString() : "",
                                                     ValDistVerUpper = pRow.get_Value(pRow.Fields.FindField("valDistVerUpper")) != null ?ToDouble (pRow.get_Value(pRow.Fields.FindField("valDistVerUpper")).ToString()) : 0,
                                                     UomDistVerUpper = pRow.get_Value(pRow.Fields.FindField("uomDistVerUpper")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerUpper")).ToString() : "",
                                                     CodeDistVerLower = pRow.get_Value(pRow.Fields.FindField("codeDistVerLower")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerLower")).ToString() : "",
                                                     ValDistVerLower = pRow.get_Value(pRow.Fields.FindField("valDistVerLower")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valDistVerLower")).ToString()) : 0,
                                                     UomDistVerLower = pRow.get_Value(pRow.Fields.FindField("uomDistVerLower")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerLower")).ToString() : "",
                                                     CodeDistVerMax = pRow.get_Value(pRow.Fields.FindField("codeDistVerMax")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerMax")).ToString() : "",
                                                     ValDistVerMax = pRow.get_Value(pRow.Fields.FindField("valDistVerMax")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valDistVerMax")).ToString()): 0,
                                                     UomDistVerMax = pRow.get_Value(pRow.Fields.FindField("uomDistVerMax")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerMax")).ToString() : "",
                                                     CodeDistVerMnm = pRow.get_Value(pRow.Fields.FindField("codeDistVerMnm")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDistVerMnm")).ToString() : "",
                                                     ValDistVerMnm = pRow.get_Value(pRow.Fields.FindField("valDistVerMnm")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valDistVerMnm")).ToString()) : 0,
                                                     UomDistVerMnm = pRow.get_Value(pRow.Fields.FindField("uomDistVerMnm")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVerMnm")).ToString() : "",
                                                     ValLowerLimit = pRow.get_Value(pRow.Fields.FindField("valLowerLimit")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valLowerLimit")).ToString()) : 0,
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_codeType = pRow.get_Value(pRow.Fields.FindField("R_codeType")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeType")).ToString() : "",
                                                     R_codeId = pRow.get_Value(pRow.Fields.FindField("R_codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("R_codeId")).ToString() : "",
                                                     Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;

                    case("OB"):

                        #region Obstacles
                        

                        table = fWksp.OpenTable("ILS");
                        pCursor = table.Search(qfilter, false);

                        pRow = pCursor.NextRow();

                        while (pRow != null)
                        {
                            AIXM45_ObjectsList.Add(new AIXM45_Obstacles
                                                 {
                                                     #region

                                                     ID = Guid.NewGuid().ToString(),
                                                     R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                                                     R_geoLat = pRow.get_Value(pRow.Fields.FindField("R_geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLat")).ToString() : "",
                                                     R_geoLong = pRow.get_Value(pRow.Fields.FindField("R_geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("R_geoLong")).ToString() : "",
                                                     txtName = pRow.get_Value(pRow.Fields.FindField("txtName")) != null ? pRow.get_Value(pRow.Fields.FindField("txtName")).ToString() : "",
                                                     txtDescrType = pRow.get_Value(pRow.Fields.FindField("txtDescrType")) != null ? pRow.get_Value(pRow.Fields.FindField("txtDescrType")).ToString() : "",
                                                     codeGroup = pRow.get_Value(pRow.Fields.FindField("codeGroup")) != null ? pRow.get_Value(pRow.Fields.FindField("codeGroup")).ToString().CompareTo("Y") ==0 : false,
                                                     codeLgt = pRow.get_Value(pRow.Fields.FindField("codeLgt")) != null ? pRow.get_Value(pRow.Fields.FindField("codeLgt")).ToString().CompareTo("Y") == 0: false,
                                                     txtDescrLgt = pRow.get_Value(pRow.Fields.FindField("txtDescrLgt")) != null ? pRow.get_Value(pRow.Fields.FindField("txtDescrLgt")).ToString() : "",
                                                     txtDescrMarking = pRow.get_Value(pRow.Fields.FindField("txtDescrMarking")) != null ? pRow.get_Value(pRow.Fields.FindField("txtDescrMarking")).ToString() : "",
                                                     CodeDatum = pRow.get_Value(pRow.Fields.FindField("codeDatum")) != null ? pRow.get_Value(pRow.Fields.FindField("codeDatum")).ToString() : "",
                                                     valGeoAccuracy = pRow.get_Value(pRow.Fields.FindField("valGeoAccuracy")) != null ?ToDouble( pRow.get_Value(pRow.Fields.FindField("valGeoAccuracy")).ToString()) : 0,
                                                     uomGeoAccuracy = pRow.get_Value(pRow.Fields.FindField("uomGeoAccuracy")) != null ? pRow.get_Value(pRow.Fields.FindField("uomGeoAccuracy")).ToString() : "",
                                                     valElev = pRow.get_Value(pRow.Fields.FindField("valElev")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valElev")).ToString()) : 0,
                                                     valElevAccuracy = pRow.get_Value(pRow.Fields.FindField("valElevAccuracy")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valElevAccuracy")).ToString()) : 0,
                                                     valHgt = pRow.get_Value(pRow.Fields.FindField("valHgt")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valHgt")).ToString()): 0,
                                                     valGeoidUndulation = pRow.get_Value(pRow.Fields.FindField("valGeoidUndulation")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valGeoidUndulation")).ToString()) : 0,
                                                     uomDistVer = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString() : "",
                                                     ValCrc = pRow.get_Value(pRow.Fields.FindField("valCrc")) != null ? pRow.get_Value(pRow.Fields.FindField("valCrc")).ToString() : "",
                                                     txtVerDatum = pRow.get_Value(pRow.Fields.FindField("txtVerDatum")) != null ? pRow.get_Value(pRow.Fields.FindField("txtVerDatum")).ToString() : "",
                                                     txtRmk = pRow.get_Value(pRow.Fields.FindField("txtRmk")) != null ? pRow.get_Value(pRow.Fields.FindField("txtRmk")).ToString() : "",

                                                     #endregion
                                                 });

                            pRow = pCursor.NextRow();

                        }


                        #endregion

                        break;
                    default:
                        break;
                }
            }



            this.Tag = AIXM45_ObjectsList;



        }

        private AIXM45_ILZ GetILZ(IFeatureWorkspace fWksp, string Ils_ID)
        {
            AIXM45_ILZ res = null;
            IQueryFilter qfilter = new QueryFilterClass();
            qfilter.WhereClause = "R_ILSMID = '" + Ils_ID+"'";

            ITable table = fWksp.OpenTable("ILZ");
            ICursor pCursor = table.Search(qfilter, false);

            IRow pRow = pCursor.NextRow();

            if (pRow != null)
            {
                res = new AIXM45_ILZ 
                {
                    ID =  pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : Guid.NewGuid().ToString(),
                    CodeId = pRow.get_Value(pRow.Fields.FindField("codeId")) != null ? pRow.get_Value(pRow.Fields.FindField("codeId")).ToString() : "",
                    ValFreq = pRow.get_Value(pRow.Fields.FindField("valFreq")) != null ?ToDouble( pRow.get_Value(pRow.Fields.FindField("valFreq")).ToString()): 0,
                    UomFreq = pRow.get_Value(pRow.Fields.FindField("uomFreq")) != null ? pRow.get_Value(pRow.Fields.FindField("uomFreq")).ToString() : "",
                    CodeEm = pRow.get_Value(pRow.Fields.FindField("codeEm")) != null ? pRow.get_Value(pRow.Fields.FindField("codeEm")).ToString() : "",
                    ValMagBrg = pRow.get_Value(pRow.Fields.FindField("valMagBrg")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valMagBrg")).ToString()) : 0,
                    ValTrueBrg = pRow.get_Value(pRow.Fields.FindField("valTrueBrg")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valTrueBrg")).ToString()) : 0,
                    ValWidCourse = pRow.get_Value(pRow.Fields.FindField("valWidCourse")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valWidCourse")).ToString()) : 0,
                    CodeTypeUseBack = pRow.get_Value(pRow.Fields.FindField("codeTypeUseBack")) != null ? pRow.get_Value(pRow.Fields.FindField("codeTypeUseBack")).ToString() : "",
                    GeoLat = pRow.get_Value(pRow.Fields.FindField("geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("geoLat")).ToString() : "",
                    GeoLong = pRow.get_Value(pRow.Fields.FindField("geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("geoLong")).ToString() : "",
                    ValElev = pRow.get_Value(pRow.Fields.FindField("valElev")) != null ? ToDouble( pRow.get_Value(pRow.Fields.FindField("valElev")).ToString()): 0,
                    UomDistVer = pRow.get_Value(pRow.Fields.FindField("uomDistVer")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString() : "",
                    R_ILSMID = pRow.get_Value(pRow.Fields.FindField("R_ILSMID")) != null ? pRow.get_Value(pRow.Fields.FindField("R_ILSMID")).ToString() : "",
                    R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                    Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry


                };

            }

            return res;
        }

        private AIXM45_IGP GetIGP(IFeatureWorkspace fWksp, string Ils_ID)
        {
            AIXM45_IGP res = null;
            IQueryFilter qfilter = new QueryFilterClass();
            qfilter.WhereClause = "R_ILSMID = '" + Ils_ID + "'";

            ITable table = fWksp.OpenTable("IGP");
            ICursor pCursor = table.Search(qfilter, false);

            IRow pRow = pCursor.NextRow();

            if (pRow != null)
            {
                res = new AIXM45_IGP
                {
                    ID = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : Guid.NewGuid().ToString(),
                    R_mid = pRow.get_Value(pRow.Fields.FindField("R_mid")) != null ? pRow.get_Value(pRow.Fields.FindField("R_mid")).ToString() : "",
                    ValFreq = pRow.get_Value(pRow.Fields.FindField("valFreq")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valFreq")).ToString()) : 0,
                    UomFreq = pRow.get_Value(pRow.Fields.FindField("uomFreq")) != null ? pRow.get_Value(pRow.Fields.FindField("uomFreq")).ToString() : "",
                    CodeEm = pRow.get_Value(pRow.Fields.FindField("codeEm")) != null ? pRow.get_Value(pRow.Fields.FindField("codeEm")).ToString() : "",
                    ValSlope = pRow.get_Value(pRow.Fields.FindField("valSlope")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valSlope")).ToString()) : 0,
                    ValRdh = pRow.get_Value(pRow.Fields.FindField("valRdh")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valRdh")).ToString()) : 0,
                    UomRdh = pRow.get_Value(pRow.Fields.FindField("uomRdh")) != null ? pRow.get_Value(pRow.Fields.FindField("uomRdh")).ToString() : "",
                    GeoLat = pRow.get_Value(pRow.Fields.FindField("geoLat")) != null ? pRow.get_Value(pRow.Fields.FindField("geoLat")).ToString() : "",
                    GeoLong = pRow.get_Value(pRow.Fields.FindField("geoLong")) != null ? pRow.get_Value(pRow.Fields.FindField("geoLong")).ToString() : "",
                    ValElev = pRow.get_Value(pRow.Fields.FindField("valElev")) != null ? ToDouble(pRow.get_Value(pRow.Fields.FindField("valElev")).ToString()) : 0,
                    UomDistVer = pRow.get_Value(pRow.Fields.FindField("uomDistVer")) != null ? pRow.get_Value(pRow.Fields.FindField("uomDistVer")).ToString() : "",
                    R_ILSMID = pRow.get_Value(pRow.Fields.FindField("R_ILSMID")) != null ? pRow.get_Value(pRow.Fields.FindField("R_ILSMID")).ToString() : "",
                    Geometry = pRow.get_Value(pRow.Fields.FindField("SHAPE")) as IGeometry

                };

            }

            return res;
        }

        private double ToDouble(string dblVal)
        {
            double res = 0;
            Double.TryParse(dblVal, out res);

            return res;
        }

        private List<AIXM45_AirspaceBorderItem> GetBorder(string arsps_R_mid)
        {
            List<AIXM45_AirspaceBorderItem> res = new List<AIXM45_AirspaceBorderItem>();

            IEnumerable<XElement> aixm45_ArspsBrdrList = from aixm45_obj in xdoc.Descendants("Abd") 
                                                         where (aixm45_obj.Element("AbdUid") != null && aixm45_obj.Element("AbdUid").Element("AseUid").Attribute("mid").Value.CompareTo(arsps_R_mid) == 0) 
                                                         select aixm45_obj;

            foreach (var item in aixm45_ArspsBrdrList)
            {
                if (item.Element("Avx") != null)
                {
                    var vrtxList = from vrtx in item.Descendants("Avx")
                                   select
                                       new AIXM45_AirspaceBorderItem
                                       {
                                           CodeType = vrtx.Element("codeType") != null && vrtx.Element("codeType").Value.CompareTo("FNT") == 0 ? BorderItemCodeType.FNT :
                                                          vrtx.Element("codeType") != null && vrtx.Element("codeType").Value.CompareTo("CWA") == 0 ? BorderItemCodeType.CWA :
                                                          vrtx.Element("codeType") != null && vrtx.Element("codeType").Value.CompareTo("CCA") == 0 ? BorderItemCodeType.CCA : BorderItemCodeType.PNT,
                                           FinalPoint = CreateWksPoint(vrtx.Element("geoLat").Value, vrtx.Element("geoLong").Value, vrtx.Element("valCrc").Value),
                                           Gbr = vrtx.Element("GbrUid") != null ? getGbrPoints(vrtx.Element("GbrUid").Attribute("mid").Value) : null,
                                           CenterPoint = vrtx.Element("codeType").Value.CompareTo("CWA") == 0 ||  vrtx.Element("codeType").Value.CompareTo("CCA") == 0?
                                                              CreateWksPoint(vrtx.Element("geoLatArc").Value, vrtx.Element("geoLongArc").Value,"") : CreateWksPoint(),
                                           Radius = vrtx.Element("valRadiusArc") != null ? Double.Parse(vrtx.Element("valRadiusArc").Value) : 0,
                                           UomRadius = vrtx.Element("uomRadiusArc") != null ? vrtx.Element("uomRadiusArc").Value : "",
                                       };

                    res.AddRange(vrtxList);
                    
                }

                if (item.Element("Circle") != null)
                {
                    var vrtxList = from vrtx in item.Descendants("Circle")
                                   select
                                       new AIXM45_AirspaceBorderItem
                                       {
                                           CodeType =  BorderItemCodeType.CRCL,
                                           CenterPoint = CreateWksPoint(vrtx.Element("geoLatCen").Value, vrtx.Element("geoLongCen").Value,""),
                                           Radius = vrtx.Element("valRadius") != null ? Double.Parse(vrtx.Element("valRadius").Value) : 0,
                                           UomRadius = vrtx.Element("uomRadius") != null ? vrtx.Element("uomRadius").Value : "",
                                       };

                     res.AddRange(vrtxList);


                }
            }

            return res;
        }


        private AIXM45_AirspaceVertex[] getGbrPoints(string xAttribute)
        {

            List<AIXM45_AirspaceVertex> resList = new List<AIXM45_AirspaceVertex>();
            
            IEnumerable<XElement> aixm45_GbrPolintsList = from aixm45_obj in xdoc.Descendants("Gbr")
                                                          where (aixm45_obj.Element("GbrUid") != null && aixm45_obj.Element("GbrUid").Attribute("mid").Value.CompareTo(xAttribute) == 0 && aixm45_obj.Element("Gbv") != null)
                                                          select aixm45_obj;


            foreach (var item in aixm45_GbrPolintsList)
            {

                if (item.Element("Gbv") != null)
                {
                    var vrtxList = from vrtx in item.Descendants("Gbv")
                                   select
                                   new AIXM45_AirspaceVertex
                                   {
                                       CrcCode = vrtx.Element("valCrc").Value,
                                       Vrtx = new WKSPoint
                                       {
                                           X = vrtx.Element("geoLong") != null ? util.GetLONGITUDEFromAIXMString(vrtx.Element("geoLong").Value) : 0,
                                           Y = vrtx.Element("geoLong") != null ? util.GetLATITUDEFromAIXMString(vrtx.Element("geoLat").Value) : 0,
                                       },

                                   };

                    
                    resList.AddRange(vrtxList);

                }
            }

            AIXM45_AirspaceVertex[] resArray = new AIXM45_AirspaceVertex[resList.Count];


            for (int i = 0; i <= resList.Count - 1; i++)
            {
                resArray[i] = resList[i];
            }
            return resArray;
        }

        private AIXM45_AirspaceVertex CreateWksPoint(string LatCoord, string LonCoord, string CrcCode)
        {
            return new AIXM45_AirspaceVertex
            {
                Vrtx = new WKSPoint
                {
                    X = util.GetLONGITUDEFromAIXMString(LonCoord),
                    Y = util.GetLATITUDEFromAIXMString(LatCoord)
                },

                CrcCode = CrcCode
            }; 
        }

        private AIXM45_AirspaceVertex CreateWksPoint()
        {
            return new AIXM45_AirspaceVertex { };
        }

        private void Prev_button_Click(object sender, EventArgs e)
        {
            HideLabelStep(wizardTabControl1.SelectedIndex);


            wizardTabControl1.SelectedIndex--;


            Prev_button.Enabled = !(wizardTabControl1.SelectedIndex == 0);
            Next_button.Enabled = true;
            button1.Enabled = false;

            ShowLabelStep(wizardTabControl1.SelectedIndex);

            button1.Enabled = false;
        }

        private void ShowLabelStep(int p)
        {
            foreach (Control cntrl in panel1.Controls)
            {
                if ((cntrl.Tag != null) && (cntrl is Label) && (Convert.ToInt32(cntrl.Tag) == p))
                {
                    (cntrl as Label).Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    (cntrl as Label).ForeColor = System.Drawing.Color.DarkOrange;

                }
            }
        }

        private void HideLabelStep(int p)
        {
            foreach (Control cntrl in panel1.Controls)
            {
                if ((cntrl.Tag != null) && (cntrl is Label) && (Convert.ToInt32(cntrl.Tag) == p))
                {
                    (cntrl as Label).Font = new System.Drawing.Font("Times New Roman", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                    (cntrl as Label).ForeColor = System.Drawing.Color.White;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DefineArea();
        }

        private void DefineArea()
        {
            if (comboBox1.Items.Count > 0) return;
            comboBox1.Items.Clear();

            string PathToSpecificationFile = Static_Proc.GetPathToSpecificationFile();
            string PathToRegionsFile = Static_Proc.GetPathToRegionsFile();

            comboBox1.Items.AddRange(AreaManager.AreaUtils.GetCountryList(PathToRegionsFile).ToArray());

            comboBox1.Tag = AreaManager.AreaUtils.GetCountryICAOCodes(PathToRegionsFile);

            //System.Diagnostics.Debug.WriteLine("DefineArea GetArea");


            Area = new AreaInfo();
            Area = AreaUtils.GetArea(PathToSpecificationFile);

            comboBox1.Text = Area.CountryName;
            comboBox2.Text = Area.Region;
            comboBox3.Text = Area.FirstLetter;
            textBox3.Text = Area.Reference_ADHP.ICAO_CODE;

        }

        private void treeView3_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            bool f = true;
            if (e.Node.GetNodeCount(true) > 0)
            {
                foreach (TreeNode nd in e.Node.Nodes) nd.Checked = e.Node.Checked;
            }

            if (e.Node.Parent != null)
            {

                foreach (TreeNode nd in e.Node.Parent.Nodes)
                    f = nd.Checked && f;

                e.Node.Parent.Checked = f;
            }

            int indx = e.Node.Index;
            f = e.Node.Checked;


            Next_button.Enabled = SetEnabledState();
        }

        private bool SetEnabledState()
        {

            bool res = false;

            foreach (TreeNode Nd in treeView3.Nodes)
            {
                if (Nd.Checked)
                {
                    res = true;
                    break;
                }


                if (Nd.Nodes.Count > 0)
                {
                    foreach (TreeNode NdTags in Nd.Nodes)
                    {
                        if (NdTags.Checked)
                        {
                            res = true;
                            break;
                        }
                    }
                }
            }

            return res && textBox1.Text.Trim().Length > 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if  (MessageBox.Show("Exit without saving results?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                Close();
            }

            //if (treeView1.Nodes.Count == 0) Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            OpenFileDialog fileDialog = new OpenFileDialog { Filter = @"Aran4.5 xml (*.xml)|*.xml|Aran4.5 mdb (*.mdb)|*.mdb" };

            if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = fileDialog.FileName;
            }

            Next_button.Enabled = SetEnabledState();
        }

        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }
            //table.Columns.Add("ObjectType");
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                //row[row.ItemArray.Length - 1] = typeof(T);
                table.Rows.Add(row);
            }
            return table;

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows == null) return;
            if (dataGridView1.SelectedRows.Count <= 0) return;

            AIXM45_Object Element = (from element in AIXM45_ObjectsList
                                       where (element != null) &&
                                           (element.ID.CompareTo(dataGridView1.SelectedRows[0].Cells["ID"].Value.ToString()) == 0)
                                       select element).First();


            propertyGrid1.SelectedObject = Element;


        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            Dictionary<string, List<string>> icao_codes = (Dictionary<string, List<string>>)comboBox1.Tag;
            comboBox3.Items.Clear();
            comboBox3.Text = "";
            if (icao_codes.ContainsKey(comboBox1.Text))
            {
                comboBox3.Items.AddRange(((List<string>)icao_codes[comboBox1.Text]).ToArray());
            }

            if (comboBox3.Items.Count > 0) comboBox3.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
 
    }

}
