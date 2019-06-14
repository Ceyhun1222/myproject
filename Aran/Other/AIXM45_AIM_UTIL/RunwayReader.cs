using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Enums;

namespace AIXM45_AIM_UTIL
{
    public class RunwayReader : Aixm45AimUtil.IAIXM45_DATA_READER
    {

        private List<ConvertedObj> _ListOfObjects;
        public List<ConvertedObj> ListOfObjects
        {
            get { return _ListOfObjects; }
            set { _ListOfObjects = value; }
        }

        public void ReadDataFromTable(string TblName, ITable ARAN_TABLE)
        {
            ListOfObjects = new List<ConvertedObj>();
            
            IQueryFilter queryFilter = new QueryFilterClass();

            queryFilter.WhereClause = ARAN_TABLE.OIDFieldName + ">= 0";

            ICursor cursor = ARAN_TABLE.Search(queryFilter, true);
            IRow Row = null;
            while ((Row = cursor.NextRow()) != null)
            {
                try
                {
                    double dbl = 0;
                    UomDistance UomDstnc;

                    Runway Rnwy = new Runway();
                    Rnwy.Identifier = Guid.NewGuid();
                    Rnwy.Type = Aran.Aim.Enums.CodeRunway.RWY;

                    Rnwy.Designator = Row.get_Value(Row.Fields.FindField("R_txtDesig")).ToString();

                    if (Enum.TryParse<UomDistance>(Row.get_Value(Row.Fields.FindField("uomDimStrip")).ToString(), true, out UomDstnc)) {

                        //Rnwy.LengthStrip
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valLenStrip")).ToString(), out dbl))
                            Rnwy.LengthStrip = new Aran.Aim.DataTypes.ValDistance(dbl, UomDstnc);

                        //Rnwy.WidthStrip
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valWidStrip")).ToString(), out dbl))
                            Rnwy.WidthStrip = new Aran.Aim.DataTypes.ValDistance(dbl, UomDstnc);

                        //Rnwy.LengthOffset
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valLenOffset")).ToString(), out dbl))
                            Rnwy.LengthOffset = new Aran.Aim.DataTypes.ValDistanceSigned(dbl, UomDstnc);

                        //Rnwy.WidthOffset
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valWidOffset")).ToString(), out dbl))
                            Rnwy.WidthOffset = new Aran.Aim.DataTypes.ValDistanceSigned(dbl, UomDstnc);
                    }

                    //Rnwy.SurfaceProperties.Composition 
                    CodeSurfaceComposition UomCmps;
                    if (Enum.TryParse<CodeSurfaceComposition>(Row.get_Value(Row.Fields.FindField("codeComposition")).ToString().Replace('+', '_'), true, out UomCmps)) {
                        Rnwy.SurfaceProperties = new SurfaceCharacteristics();
                        Rnwy.SurfaceProperties.Composition = UomCmps;
                    }

                    //Rnwy.NominalLength
                    if (Enum.TryParse<UomDistance>(Row.get_Value(Row.Fields.FindField("uomDimRwy")).ToString(), true, out UomDstnc)) {
                        if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valLen")).ToString(), out dbl))
                            Rnwy.NominalLength = new Aran.Aim.DataTypes.ValDistance(dbl, UomDstnc);
                    }

                    //Rnwy.NominalWidth
                    if (Double.TryParse(Row.get_Value(Row.Fields.FindField("valWid")).ToString(), out dbl))
                        Rnwy.NominalWidth = new Aran.Aim.DataTypes.ValDistance(dbl, UomDstnc);

                    ListOfObjects.Add(new ConvertedObj(Row.get_Value(Row.Fields.FindField("R_mid")).ToString(), Row.get_Value(Row.Fields.FindField("R_AHPmid")).ToString(), Rnwy));
                }
                catch
                {
                    continue;
                }
            }

        }
    }
}
