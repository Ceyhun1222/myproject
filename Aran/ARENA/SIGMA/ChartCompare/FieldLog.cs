using ESRI.ArcGIS.Geometry;

namespace ChartCompare
{
    public class FieldLog
    {
		public FieldLog (string fieldName, string oldVal, string newVal)
		{
			FieldName = fieldName;
			OldValueText = oldVal;
			NewValueText = newVal;
		}

        public string FieldName { get; set; }
        public string ChangeText { get; set; }

        public string OldValueText { get; }
        public string NewValueText { get; set; }

        public IGeometry OldGeometry { get; set; }
    }
}
