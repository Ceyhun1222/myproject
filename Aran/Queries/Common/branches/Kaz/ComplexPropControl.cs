using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Aim.DataTypes;
using Aran.Aim.Objects;

namespace Aran.Queries.Common
{
    public partial class ComplexPropControl : UserControl
    {
        private AimPropInfo _propInfo;
        public event EntityClickedHandler EntityClicked;
        public event GetFeatureHandler GetFeature;

        public ComplexPropControl ()
        {
            InitializeComponent ();
        }

        public IAimProperty Value { get; set; }

        public AimPropInfo PropInfo
        {
            get { return _propInfo; }
            set
            {
                _propInfo = value;

                if (AimMetadata.IsChoice (_propInfo.TypeIndex))
                {
                    #region Choice Class

                    IAimObject aimObject = (IAimObject) Value;
                    AimPropInfo [] propInfoArr = AimMetadata.GetAimPropInfos (aimObject);

                    IEditChoiceClass choiceClass = (IEditChoiceClass) aimObject;
                    AimObjectType aot = AimMetadata.GetAimObjectType (choiceClass.RefType);
                    AimPropInfo api = null;

                    if (aot == AimObjectType.Feature)
                    {
                        api = propInfoArr.Where (pi => pi.IsFeatureReference && 
                                pi.ReferenceFeature == (FeatureType) choiceClass.RefType).First ();
                    }
                    else if (aot == AimObjectType.Object || aot == AimObjectType.DataType)
                    {
                        api = propInfoArr.Where (pi => pi.TypeIndex == choiceClass.RefType).First ();
                    }

                    if (api != null)
                    {
                        propNameLinkLabel.Text = _propInfo.AixmName + "_" + api.AixmName;
                    }

                    #endregion
                }
                else
                {
                    propNameLinkLabel.Text = PropControl.MakeSentence (_propInfo.AixmName);
                }

                this.Width = propNameLinkLabel.Width + 40;
            }
        }

        public void NodeTag_DBEntityCleared (object sender, EventArgs e)
        {
            NodeTag nodeTag = (NodeTag) sender;

            IAimObject parentObject = nodeTag.Parent.Entity;
            parentObject.SetValue (PropInfo.Index, null);

            Parent.Controls.Remove (this);
            this.Dispose ();
        }

        private void propNameLinkLabel_LinkClicked (object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (EntityClicked != null)
            {
                if (Value.PropertyType == AimPropertyType.DataType)
                {
                    if (GetFeature != null)
                    {
                        ADataType dataType = (ADataType) Value;
                        Feature feature = null;

                        if (dataType.DataType == DataType.FeatureRef)
                        {
                            FeatureRef featRef = dataType as FeatureRef;
                            feature = GetFeature (_propInfo.ReferenceFeature, featRef.Identifier);
                        }
                        else if (AimMetadata.IsAbstractFeatureRef ((int) dataType.DataType))
                        {
                            IAbstractFeatureRef absFeatRef = dataType as IAbstractFeatureRef;
                            feature = GetFeature ((FeatureType) absFeatRef.FeatureTypeIndex, absFeatRef.Identifier);
                        }

                        if (feature != null)
                        {
                            EntityClicked (this, feature as DBEntity, _propInfo.AixmName, true);
                        }
                    }
                }
                else
                {
                    EntityClicked (this, Value as DBEntity, _propInfo.AixmName, true);
                }
            }
        }

        private Feature OnGetFeature (FeatureType featureType, Guid identifier)
        {
            if (GetFeature != null)
            {
                return GetFeature (featureType, identifier);
            }
            return null;
        }
    }
}
