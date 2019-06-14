using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Threading;
using Aran.Aim;
using Aran.Geometries;
using MvvmCore;
using TOSSM.Converter;
using TOSSM.Geometry;

namespace TOSSM.ViewModel.Document.Single.Editable
{
    public class ReflectionEditablePropertyModel : EditableSinglePropertyModel
    {
        #region Property related

        private void PrepareRelatedData()
        {
            if (PropertyInfo == null) return;

            if (PropertyInfo.PropertyType.IsEnum)
            {
                //prerare enum

                EnumList = new List<EnumViewModel>();
                foreach (var en in Enum.GetValues(PropertyInfo.PropertyType))
                {
                    EnumList.Add(new EnumViewModel { Enum = en });
                }

               
            }
           

            if (PropertyInfo.Name=="Geo")
            {
                PictureId = GeoPicture;
            }
            else
            {
                 PictureId = ComplexPicture;
            }
           
        }

        private PropertyInfo _propertyInfo;
        private string _geoTextValue;
        public string GeoTextValue
        {
            get => _geoTextValue;
            set
            {
                _geoTextValue = value;
                OnPropertyChanged("GeoTextValue");
            }
        }

        public PropertyInfo PropertyInfo
        {
            get => _propertyInfo;
            set
            {
                _propertyInfo = value;
                PropertyName = PropertyInfo.Name;
                PrepareRelatedData();
                GetValueFromParent();
            }
        }

        #endregion

        #region Overrides of EditableSinglePropertyModel

        public override void GetValueFromParent()
        {
            if (PropertyInfo==null) return;
            if (ParentObject == null) return;

            Value = PropertyInfo.GetValue(ParentObject, null);

            if (PropertyInfo.PropertyType.IsEnum)
            {
                UpdateEnumValue();
            }


            var worker = new BackgroundWorker();
            worker.DoWork += (a, b) =>
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                StringValue = HumanReadableConverter.ToHuman(Value);
            };
            worker.RunWorkerAsync();
        }


        public override bool CanPaste()
        {
            if (PropertyName=="Geo")
            {
                var geoValue = Value as Aran.Geometries.Geometry;

                if (geoValue == null) return false;
                if (MainManagerModel.Instance.Clipboard.ClipboardAranGeometry == null) return false;
                if (MainManagerModel.Instance.Clipboard.Count > 1) return false;

                return MainManagerModel.Instance.Clipboard.ClipboardAranGeometry.Type == geoValue.Type;
            }
            else
            {
                return base.CanPaste();
            }
        }

        public override void Paste()
        {
            if (PropertyName == "Geo")
            {
                var currentGeo = Value as Aran.Geometries.Geometry;
                if (currentGeo!=null)
                {
                    GeoFormatter.CopyGeometry(MainManagerModel.Instance.Clipboard.ClipboardAranGeometry, currentGeo);

                    MainManagerModel.Instance.OnMapViewerToolViewModel.SelectGeometryOnMap(currentGeo);

                }

                if (ParentModel!=null)
                {
                    ParentModel.UpdateByChildren();
                    ParentModel.UpdateComplexContent();
                }
            }
            else
            {
                base.Paste();
            }
        }

        private RelayCommand _clearGeo;
        public RelayCommand ClearGeo
        {
            get { return _clearGeo??(_clearGeo=new RelayCommand(
                t=>
                    {
                         var currentGeo = Value as Aran.Geometries.Geometry;
                         if (currentGeo != null)
                         {
                             //clear old
                             var clearMethod = currentGeo.GetType().GetMethod("Clear");
                             if (clearMethod!=null)
                             {
                                 clearMethod.Invoke(currentGeo, null);
                             }

                             //create new
                             AimField aimField = null;
                             switch (currentGeo.Type)
                             {
                                 case GeometryType.Point:
                                     aimField = AimObjectFactory.CreateAimField(AimFieldType.GeoPoint);
                                     break;
                                 case GeometryType.Polygon:
                                     aimField = AimObjectFactory.CreateAimField(AimFieldType.GeoPolygon);
                                     break;
                                 case GeometryType.Line:
                                 case GeometryType.LineString:
                                     aimField = AimObjectFactory.CreateAimField(AimFieldType.GeoPolyline);
                                     break;
                             }
                             if (aimField!=null)
                             {
                                 var newGeo = ((dynamic)aimField).Value;
                                 GeoFormatter.CopyGeometry(newGeo, currentGeo);
                             }

                             MainManagerModel.Instance.OnMapViewerToolViewModel.SelectFeatureOnMap(null);

                             if (ParentModel != null)
                             {
                                 ParentModel.UpdateByChildren();
                                 ParentModel.UpdateComplexContent();
                             }
                         }

                       
                    }));
            }
        }


        public override void SetValueToParent(bool fromUser=true)
        {
            if (PropertyInfo == null) return;
            if (ParentObject == null) return;

            if (!PropertyInfo.CanWrite)
            {
                return;
            }


            //set value
            PropertyInfo.SetValue(ParentObject, Value, null);

            CheckIsNull();

            OnPropertyChanged("IsChanged");

            OnPropertyChanged("ChangedVisibility");
            OnPropertyChanged("NullReasonVisibility");
            OnPropertyChanged("MainValueVisibility");

            if (PropertyInfo.PropertyType.IsEnum)
            {
                UpdateEnumValue();
            }

            //update parent
            //string value will be updated as well
            if (fromUser && ParentModel != null)
            {
                if (ParentModel.SelectedProperty == this)
                {
                    if (fromUser)
                    {
                        ParentModel.UpdateByChildren();
                        ParentModel.UpdateSelected();
                    }
                }
            }
        }

        public override void UpdateStringValue()
        {
            
                var worker = new BackgroundWorker();
                worker.DoWork += (a, b) =>
                {
                    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
                    StringValue = HumanReadableConverter.ToHuman(Value);
                };
                worker.RunWorkerAsync();
            
        }

        public override void SetNew()
        {
            if (PropertyInfo != null)
            {
                Value = Activator.CreateInstance(PropertyInfo.PropertyType);
            }
        }

        #endregion
    }
}
