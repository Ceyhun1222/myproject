using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using System.Xml;
using AranSupport;
using System.IO;
using System.Collections;
using Microsoft.Win32;

namespace AreaManager
{
    public static class AreaUtils
    {
        public static AreaInfo GetArea(string PathToAreaXml)
        {

            AranSupport.Utilitys AranUtilS = new AranSupport.Utilitys();

            System.IO.FileStream RegionDoc_FILE = new System.IO.FileStream(PathToAreaXml, FileMode.Open);
            XmlDocument RegionDoc_XML = new XmlDocument(); ;
            AreaInfo areaInf = new AreaInfo();

            RegionDoc_XML.Load(RegionDoc_FILE);

            XmlNode RegionNode = RegionDoc_XML.SelectSingleNode("./List/Region");
            areaInf.Region = RegionNode.FirstChild.Value.Trim();

            XmlNodeList CountryList = RegionDoc_XML.SelectNodes("//Region/Country");

            for (int i = 0; i < CountryList.Count; i++)
            {

                XmlNode XML_COUNTRY_Node = CountryList[i];


                areaInf.CountryId = XML_COUNTRY_Node.Attributes["ID"].Value;
                areaInf.CountryName = XML_COUNTRY_Node.Attributes["Name"].Value;
                areaInf.FirstLetter = XML_COUNTRY_Node.Attributes["FirstLetter"].Value;

                XmlNode XML_adhp_Node = XML_COUNTRY_Node.SelectSingleNode("ADHP");
                areaInf.Reference_ADHP.ICAO_CODE = XML_adhp_Node.InnerText;
                areaInf.Reference_ADHP.Lat = XML_adhp_Node.Attributes[0].Value;
                areaInf.Reference_ADHP.Lon = XML_adhp_Node.Attributes[1].Value;

                areaInf.AreaPolygon = AranUtilS.GetObjectFromBlob(XML_COUNTRY_Node.SelectSingleNode("AREA").InnerText, "SHAPE") as IGeometry;

                if ((areaInf.Reference_ADHP.ICAO_CODE.Trim().Length > 0) && (areaInf.Reference_ADHP.Lat.Trim().Length > 0) && (areaInf.Reference_ADHP.Lon.Trim().Length > 0))
                {

                    //В морской миле 1852,4 м. Одна морская миля - это одна минута дуги земного меридиана. 
                    // Таким образом, получается, что в одной минуте дуги меридиана на любой карте – морской ли, сухопутной - один километр, восемьсот пятьдесят два метра и сорок сантиметров. 
                    //В одном градусе соответственно: 1852.4 х 60 =111144 метра или 111 км, 144 м


                    object missing = Type.Missing;
                    IPoint pnt = AranUtilS.Create_ESRI_POINT(areaInf.Reference_ADHP.Lat.Trim(), areaInf.Reference_ADHP.Lon.Trim(), "0", "FT");
                    ICircularArc circle = new CircularArcClass();
                    IConstructCircularArc const_circle = (IConstructCircularArc)circle;
                    const_circle.ConstructCircle(pnt, 0.7, true);
                    circle.SpatialReference = areaInf.AreaPolygon.SpatialReference;

                   

                    Polygon plgn = new PolygonClass();
                    ISegmentCollection segcol = (ISegmentCollection)plgn;
                    segcol.AddSegment((ISegment)circle, ref missing, ref missing);

                    areaInf.AreaPolygon = plgn as IGeometry;
                }



            }

            RegionDoc_FILE.Close();

            return areaInf;
        }

        public static List<string> GetCountryList(string PathToRegionsFile)
        {
            List<string> res = new List<string>();


            System.IO.FileStream region_FILE = new System.IO.FileStream(PathToRegionsFile, FileMode.Open);
            XmlDocument region_XML = new XmlDocument();
            region_XML.Load(region_FILE);


            XmlNodeList ndList = region_XML.SelectNodes("//Country/@Name");

            foreach (XmlNode nd in ndList)
                res.Add(nd.InnerText);

            region_FILE.Close();


            
            return res;
        }

        public static Dictionary<string, List<string>> GetCountryICAOCodes(string PathToRegionsFile)
        {
            Dictionary<string, List<string>> res = new Dictionary<string, List<string>>();
            System.IO.FileStream region_FILE = new System.IO.FileStream(PathToRegionsFile, FileMode.Open);
            XmlDocument region_XML = new XmlDocument();
            region_XML.Load(region_FILE);


            XmlNodeList ndList = region_XML.SelectNodes("//Country");
            if (ndList != null)
            {
                foreach (XmlNode nd in ndList)
                {
                    List<string> icao_codes = new List<string>();
                    XmlNodeList codeList = nd.SelectNodes("ICAO_CODE");

                    if (codeList != null)
                    {
                        foreach (XmlNode code in codeList)
                        {
                            icao_codes.Add(code.InnerText);
                        }

                    }

                    res.Add(nd.Attributes["Name"].Value, icao_codes);
                }
            }
            region_FILE.Close();




            return res;

        }

        public static bool _WriteToAreaFile(string PathToRegionsFile, string PathToSpecificationFile, string Region,string CntryName, string CntryFirstLetter)
        {
            
            try
            {
                System.IO.FileStream region_FILE = new System.IO.FileStream(PathToRegionsFile, FileMode.Open);
                XmlDocument region_XML = new XmlDocument();
                region_XML.Load(region_FILE);

                XmlNode nd = region_XML.SelectSingleNode("//Country[@Name='" + CntryName + "']");

                if (nd != null)
                {

                    System.IO.FileStream areaDoc_FILE = new System.IO.FileStream(PathToSpecificationFile, FileMode.Open);


                    XmlDocument RegionDoc_XML = new XmlDocument();
                    RegionDoc_XML.Load(areaDoc_FILE);

                    XmlNode reg = RegionDoc_XML.SelectSingleNode("//Region");

                    reg.FirstChild.InnerText = Region;

                    reg = RegionDoc_XML.SelectSingleNode("//Region/Country");

                    reg.Attributes["ID"].Value = nd.Attributes["ID"].Value;
                    reg.Attributes["Name"].Value = nd.Attributes["Name"].Value;
                    reg.Attributes["FirstLetter"].Value = CntryFirstLetter;
                    reg.FirstChild.InnerText = nd.FirstChild.InnerText;


                    areaDoc_FILE.Close();
                    RegionDoc_XML.Save(PathToSpecificationFile);

                }


                region_FILE.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool WriteToAreaFile(string PathToRegionsFile, string PathToSpecificationFile, AreaInfo area)
        {

            try
            {
                string Region = area.Region;
                string CntryName = area.CountryName;
                string CntryFirstLetter = area.FirstLetter;

                System.IO.FileStream region_FILE = new System.IO.FileStream(PathToRegionsFile, FileMode.Open);
                XmlDocument region_XML = new XmlDocument();
                region_XML.Load(region_FILE);

                XmlNode nd = region_XML.SelectSingleNode("//Country[@Name='" + CntryName + "']");

                if (nd != null)
                {

                    System.IO.FileStream areaDoc_FILE = new System.IO.FileStream(PathToSpecificationFile, FileMode.Open);


                    XmlDocument RegionDoc_XML = new XmlDocument();
                    RegionDoc_XML.Load(areaDoc_FILE);

                    XmlNode reg = RegionDoc_XML.SelectSingleNode("//Region");

                    reg.FirstChild.InnerText = Region;

                    reg = RegionDoc_XML.SelectSingleNode("//Region/Country");

                    reg.Attributes["ID"].Value = nd.Attributes["ID"].Value;
                    reg.Attributes["Name"].Value = nd.Attributes["Name"].Value;
                    reg.Attributes["FirstLetter"].Value = CntryFirstLetter;
                    reg.FirstChild.InnerText = nd.SelectSingleNode("AREA").InnerText;
                    reg.ChildNodes[1].InnerText = area.Reference_ADHP.ICAO_CODE;
                    reg.ChildNodes[1].Attributes[0].Value = area.Reference_ADHP.Lat;
                    reg.ChildNodes[1].Attributes[1].Value = area.Reference_ADHP.Lon;

                    areaDoc_FILE.Flush();
                    areaDoc_FILE.Close();
                    RegionDoc_XML.Save(PathToSpecificationFile);
                }

                region_FILE.Flush();
                region_FILE.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }

    public class ADHP
    {
        private string _ICAO_CODE;
        public string ICAO_CODE
        {
            get { return _ICAO_CODE; }
            set { _ICAO_CODE = value; }
        }

        private string _Lon;
        public string Lon
        {
            get { return _Lon; }
            set { _Lon = value; }
        }

        private string _Lat;
        public string Lat
        {
            get { return _Lat; }
            set { _Lat = value; }
        }

        public ADHP()
        {
        }
    }

    public class AreaInfo
    {
        private string _region;
        public string Region
        {
            get { return _region; }
            set { _region = value; }
        }

        private string _countryId;
        public string CountryId
        {
            get { return _countryId; }
            set { _countryId = value; }
        }

        private string _countryName;
        public string CountryName
        {
            get { return _countryName; }
            set { _countryName = value; }
        }

        private IGeometry _areaPolygon;
        public IGeometry AreaPolygon
        {
            get { return _areaPolygon; }
            set { _areaPolygon = value; }
        }

        private string _FirstLetter;
        public string FirstLetter
        {
            get { return _FirstLetter; }
            set { _FirstLetter = value; }
        }

        private ADHP _Reference_ADHP;
        public ADHP Reference_ADHP
        {
            get { return _Reference_ADHP; }
            set { _Reference_ADHP = value; }
        }

        public AreaInfo()
        {
            this.Reference_ADHP = new ADHP();
        }
    }

}