using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PDM.PropertyExtension;
using ARINC_DECODER_CORE.AIRTRACK_Objects;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace PDM
{
	[TypeConverter(typeof(PropertySorter))]
	public class AREA_PDM : PDMObject
	{


		private string _Name;
		[Browsable(false)]
		public string Name
		{
			get { return _Name; }
			set { _Name = value; }
		}

        [Browsable(false)]
        public override string TypeName
        {
            get
            {
                return PDM_ENUM.AREA_PDM.ToString();
            }
        } 

        public AREA_PDM()
        {
           
        }

		public AREA_PDM(Object_AIRTRACK objAirtrack)
		{
			this.Geo = objAirtrack.Shape.Geometry;
			this.Name = ((AREA_AIRTRACK)objAirtrack).AreaName;
		}

		public override bool StoreToDB(ESRI.ArcGIS.Geodatabase.ITable table)
		{
			bool res = true;
			try
			{
				IWorkspaceEdit workspaceEdit = (IWorkspaceEdit)(table as FeatureClass).Workspace;
				workspaceEdit.StartEditing(false);
				workspaceEdit.StartEditOperation();

				IRow row = table.CreateRow();

				CompileRow(ref row);

				row.Store();

				workspaceEdit.StopEditOperation();
				workspaceEdit.StopEditing(true);
			}
			catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }
			{
				res = false;
			}

			return res;
		}

		public override bool StoreToDB(Dictionary<Type, ITable> AIRTRACK_TableDic)
		{
			bool res = true;
			try
			{
				ITable tbl = AIRTRACK_TableDic[this.GetType()];
				IRow row = tbl.CreateRow();

				CompileRow(ref row);

				row.Store();
			}
			catch (Exception ex) { this.ExeptionDetails = new ExeptionMessage { Message = ex.Message, Source = ex.Source, StackTrace = ex.StackTrace }; res = false; }

			return res;
		}

		public override void CompileRow(ref ESRI.ArcGIS.Geodatabase.IRow row)
		{
			int findx = -1;

			findx = row.Fields.FindField("AreaName"); if (findx >= 0) row.set_Value(findx, this.Name);
			findx = row.Fields.FindField("Shape"); if ((findx >= 0) && (this.Geo != null)) row.set_Value(findx, this.Geo);

		}

	}
}
