using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;

namespace Aran.Queries.Common
{
    public delegate Feature GetFeatureHandler(FeatureType featureType, Guid identifier);
    public delegate IEnumerable<Feature> GetFeatureListHandler(FeatureType featureType, Aran.Aim.Data.Filters.Filter filter);
    public delegate void FeatureListByDependEventHandler(object sender, FeatureListByDependEventArgs e);
    public delegate void EntityClickedHandler(object sender, DBEntity entity, string nodeText, bool showType, int listIndex = -1);
    public delegate FeatureRef FeatureSelectedEventHandner(AimPropInfo propInfo);
    public delegate void FillDataGridColumnsHandler(AimClassInfo classInfo, DataGridView dgv);
    public delegate void SetDataGridRowHandler(DataGridView dgv, Feature feature, int rowIndex = -1);
    public delegate void FeatureEventHandler(object sender, FeatureEventArgs e);
    public delegate void SessionGeometriesEventHandler(object sender, SessionGeometriesEventArgs e);

    public class FeatureListByDependEventArgs : EventArgs
    {
        public FeatureListByDependEventArgs(
                    FeatureType featType,
                    TimeSliceInterpretationType interpretationType,
                    DateTime effectiveDate,
                    Feature dependFeat)
        {
            FeatureType = featType;
            InterpretationType = interpretationType;
            EffectiveDate = effectiveDate;
            DependFeature = dependFeat;

            FeatureList = new List<Feature>();
        }

        public List<Feature> FeatureList { get; private set; }

        public FeatureType FeatureType { get; private set; }

        public TimeSliceInterpretationType InterpretationType { get; private set; }

        public DateTime EffectiveDate { get; private set; }

        public Feature DependFeature { get; private set; }
    }

    public class FeatureEventArgs : EventArgs
    {
        public FeatureEventArgs(Feature feature)
        {
            Feature = feature;
        }

        public Feature Feature { get; private set; }
    }

    public class SessionGeometriesEventArgs : EventArgs
    {
        public SessionGeometriesEventArgs()
        {
            GeometriesDict = new Dictionary<string, Geometries.Geometry>();
        }

        public Dictionary<string, Geometries.Geometry> GeometriesDict { get; private set; }
    }

    public class PropControlTag
    {
        public PropControlTag ()
        {
        }

        public PropControlTag (IAimObject aimObject, AimPropInfo propInfo)
        {
            AimObject = aimObject;
            PropInfo = propInfo;
            PropValue = (aimObject as IAimObject).GetValue (propInfo.Index);
        }

        public IAimObject AimObject { get; set; }
        public AimPropInfo PropInfo { get; set; }
        public IAimProperty PropValue { get; set; }
    }

    public class ToolStripTag
    {
        public NodeTag NodeTag { get; set; }
        public AimPropInfo PropInfo { get; set; }
        public bool IsChoice { get; set; }
    }
}