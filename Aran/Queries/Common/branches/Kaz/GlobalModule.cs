using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;
using System.Windows.Forms;
using Aran.Aim.Features;
using Aran.Aim.Objects;
using Aran.Aim.DataTypes;

namespace Aran.Queries.Common
{
    public delegate Feature GetFeatureHandler (FeatureType featureType, Guid identifier);
    public delegate IEnumerable<Feature> GetFeatureListHandler (FeatureType featureType);
	public delegate Feature[] GetFeatureListByDependHandler(FeatureType featType, Feature dependFeat);
    public delegate void EntityClickedHandler (object sender, DBEntity entity, string nodeText, bool showType, int listIndex = -1);
    public delegate FeatureRef FeatureSelectedEventHandner (AimPropInfo propInfo);
    public delegate void FillDataGridColumnsHandler (AimClassInfo classInfo, DataGridView dgv);
    public delegate void SetDataGridRowHandler (DataGridView dgv, Feature feature, int rowIndex = -1);
    public delegate void FeatureEventHandler (object sender, FeatureEventArgs e);

    public class FeatureEventArgs : EventArgs
    {
        public FeatureEventArgs (Feature feature)
        {
            Feature = feature;
        }

        public Feature Feature { get; private set; }
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
    }
}
