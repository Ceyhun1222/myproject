using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PDM
{
    public class MarkingElement:PDMObject
    {
        [XmlElement]
        [Browsable(false)]
        public string Marking_ID { get; set; }

        [XmlElement]
        [Browsable(false)]
        public ColourType Colour { get; set; }

        [XmlElement]
        [Browsable(false)]
        public CodeMarkingStyleType Style { get; set; }

        [XmlElement]
        [Browsable(false)]
        public string MarkedElement { get; set; }


        public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
        {
            bool res = true;
            try
            {
                if (this.Geo != null)
                {
                    var findx = -1;
                    this.ID = Guid.NewGuid().ToString();
                    switch (this.Geo.GeometryType)
                    {
                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPoint):
                                                        
                            ITable tblGeoPnt = AIRTRACK_TableDic[typeof(Marking_Point)];
                            IRow rowGeoPnt = tblGeoPnt.CreateRow();
                            findx = -1;
                            findx = rowGeoPnt.Fields.FindField("FeatureGUID"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.ID);
                            findx = rowGeoPnt.Fields.FindField("Marking_ID"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.Marking_ID);
                            findx = rowGeoPnt.Fields.FindField("ActualDate"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.ActualDate);
                            findx = rowGeoPnt.Fields.FindField("Color"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.Colour.ToString());
                            findx = rowGeoPnt.Fields.FindField("Style"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.Style.ToString());
                            findx = rowGeoPnt.Fields.FindField("MarkedElement"); if (findx >= 0) rowGeoPnt.set_Value(findx, this.MarkedElement);
                            findx = rowGeoPnt.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) rowGeoPnt.set_Value(findx, this.Geo);

                            rowGeoPnt.Store();

                            break;

                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolyline):
                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryLine):
                           
                            ITable tblGeoLn = AIRTRACK_TableDic[typeof(Marking_Curve)];
                            IRow rowGeoLn = tblGeoLn.CreateRow();
                            findx = -1;
                            findx = rowGeoLn.Fields.FindField("FeatureGUID"); if (findx >= 0) rowGeoLn.set_Value(findx, this.ID);
                            findx = rowGeoLn.Fields.FindField("Marking_ID"); if (findx >= 0) rowGeoLn.set_Value(findx, this.Marking_ID);
                            findx = rowGeoLn.Fields.FindField("ActualDate"); if (findx >= 0) rowGeoLn.set_Value(findx, this.ActualDate.ToString());
                            findx = rowGeoLn.Fields.FindField("Color"); if (findx >= 0) rowGeoLn.set_Value(findx, this.Colour.ToString());
                            findx = rowGeoLn.Fields.FindField("Style"); if (findx >= 0) rowGeoLn.set_Value(findx, this.Style.ToString());
                            findx = rowGeoLn.Fields.FindField("MarkedElement"); if (findx >= 0) rowGeoLn.set_Value(findx, this.MarkedElement);
                            findx = rowGeoLn.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) rowGeoLn.set_Value(findx, this.Geo);

                            rowGeoLn.Store();

                            break;

                        case (ESRI.ArcGIS.Geometry.esriGeometryType.esriGeometryPolygon):

                            
                            ITable tblGeoPlg = AIRTRACK_TableDic[typeof(Marking_Surface)];
                            IRow rowGeoPlg = tblGeoPlg.CreateRow();
                            findx = -1;
                            findx = rowGeoPlg.Fields.FindField("FeatureGUID"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.ID);
                            findx = rowGeoPlg.Fields.FindField("Marking_ID"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.Marking_ID);
                            findx = rowGeoPlg.Fields.FindField("ActualDate"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.ActualDate.ToString());
                            findx = rowGeoPlg.Fields.FindField("Color"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.Colour.ToString());
                            findx = rowGeoPlg.Fields.FindField("Style"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.Style.ToString());
                            findx = rowGeoPlg.Fields.FindField("MarkedElement"); if (findx >= 0) rowGeoPlg.set_Value(findx, this.MarkedElement);
                            findx = rowGeoPlg.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) rowGeoPlg.set_Value(findx, this.Geo);

                            rowGeoPlg.Store();

                            break;
                    }
                }

            }
            catch (Exception ex)
            {
                this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace };
                res = false;
            }

            return res;

        }

        public override void CompileRow(ref IRow row)
        {
            int findx = -1;

            findx = row.Fields.FindField("FeatureGUID"); if (findx >= 0) row.set_Value(findx, this.ID);
            findx = row.Fields.FindField("Marking_ID"); if (findx >= 0) row.set_Value(findx, this.Marking_ID);
            findx = row.Fields.FindField("ActualDate"); if (findx >= 0) row.set_Value(findx, this.ActualDate);
            //findx = row.Fields.FindField("VisibilityFlag"); if (findx >= 0) row.set_Value(findx, this.VisibilityFlag);


        }
    }
}
