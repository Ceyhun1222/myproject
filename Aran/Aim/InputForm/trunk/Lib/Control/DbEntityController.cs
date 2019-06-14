using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim.Objects;
using Aran.Aim.Metadata.UI;

namespace Aran.Aim.InputFormLib
{
    public delegate IAimFieldControl CreatorAimFieldControlHandler ();
    public delegate IAObjectControl CreateAObjectControlHandler ();

    public class DbEntityController
    {
        public DbEntityController ()
        {
            
        }

        public void Load2 (DBEntity dbEntity, AimClassInfo classInfo)
        {
            _dbEntity = dbEntity;
            _classInfo = classInfo;
            _uiClassInfo = classInfo.UiInfo ();

            IAimObject aimObject = dbEntity;


            for (int i = 0; i < classInfo.Properties.Count; i++)
            {
                AimPropInfo propInfo = classInfo.Properties [i];

                if (propInfo.Index == (int) Aim.PropertyEnum.PropertyDBEntity.Id)
                    continue;

                if (propInfo.Index == (int) Aim.PropertyEnum.PropertyFeature.Identifier)
                    continue;

                AimObjectType aimObjectType = AimMetadata.GetAimObjectType (propInfo.TypeIndex);

                IAimProperty aimProp = null;
                if (aimObject != null)
                    aimProp = aimObject.GetValue (propInfo.Index);

                if (aimObjectType == AimObjectType.Field)
                {
                    IAimFieldControl fieldControl = CreateAimFieldControl ();
                    fieldControl.SetAimField (aimProp as AimField, propInfo);
                }
                else if (aimObjectType == AimObjectType.Object)
                {
                    if (propInfo.IsList)
                    {
                    }
                    else
                    {
                        IAObjectControl objectControl = CreateAObjectControl ();
                        objectControl.LoadObject ((AObject) aimProp, propInfo.PropType);
                    }
                }
            }
        }

        public DBEntity DbEntity
        {
            get { return _dbEntity; }
        }

        public string Title
        {
            get { return _uiClassInfo.Caption; }
        }

        public event CreatorAimFieldControlHandler CreateAimFieldControl;
        public event CreateAObjectControlHandler CreateAObjectControl;
        
        private UIClassInfo _uiClassInfo;
        private AimClassInfo _classInfo;
        private DBEntity _dbEntity;
    }
}
