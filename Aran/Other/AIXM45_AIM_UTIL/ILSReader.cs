using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.esriSystem;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.Enums;
using Aran.Geometries;
using Aran.Aim.DataTypes;

namespace AIXM45_AIM_UTIL
{
    public class ILSReader : Aixm45AimUtil.IAIXM45_DATA_READER
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
                    Navaid navaid = new Navaid();
                    navaid.Type = CodeNavaidService.ILS;
                    navaid.Name = Row.get_Value(Row.Fields.FindField("R_DmeMid")).ToString();
                    navaid.Identifier = Guid.NewGuid();
                    navaid.Designator = Row.get_Value(Row.Fields.FindField("R_codeIdTxt")).ToString();
                    
                    ListOfObjects.Add(new ConvertedObj(Row.get_Value(Row.Fields.FindField("R_mid")).ToString(), Row.get_Value(Row.Fields.FindField("R_RdnMid")).ToString(), navaid));
                }
                catch
                {
                    continue;
                }

            }

        }
    }
}
