using System;
using Aran.Aim.Enums;
using ESRI.ArcGIS.Geodatabase;

namespace Aran.Omega.VSImporter
{
    public static class FeatureClassExtension
    {
        public static T GetValue<T>(this IFeature feat,string fieldName)
        {
            var fieldIndex =feat.Fields.FindField(fieldName);
            if (fieldIndex < 0) return default(T) ;

            var value = feat.Value[fieldIndex];
            if (value is DBNull)
                return default(T);
            return (T) value;
        }

        public static CodeStatusConstruction? GetConstructionStatus(this IFeature feat)
        {
            var status =feat.GetValue<string>(FieldNames.Status);
            if (status == null)
                return null;

            var lowerCaseStatus = status.ToLower();
            if (lowerCaseStatus == "completed")
                return CodeStatusConstruction.COMPLETED;
            else if (lowerCaseStatus == "under construction")
                return CodeStatusConstruction.IN_CONSTRUCTION;
            else if (lowerCaseStatus == "Planned")
                return CodeStatusConstruction.DEMOLITION_PLANNED;

            return null;
        }

        
        public static CodeVerticalDatum? VerticalDatum(this IFeature feat)
        {
            var verticalDatum = feat.GetValue<string>(FieldNames.VRefSys);
            if (verticalDatum == "EGM_08")
                return CodeVerticalDatum.OTHER_EGM_08;

            if ((Enum.TryParse(verticalDatum, true, out CodeVerticalDatum result)))
                return result;

            return null;
        }
    }
}