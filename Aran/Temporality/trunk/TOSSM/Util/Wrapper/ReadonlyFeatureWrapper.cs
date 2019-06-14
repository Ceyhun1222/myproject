using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Aran.Aim;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Entity.Util;
using Aran.Temporality.Common.Util;
using TOSSM.Converter;

namespace TOSSM.Util.Wrapper
{


    public class ReadonlyFeatureWrapper : DynamicObject, INotifyPropertyChanged
    {
        public Guid Identifier { get; set; }

        private bool _isChecked;
        public bool IsChecked 
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
               OnPropertyChanged(nameof(IsChecked));
                OnFeatureChecked?.Invoke(this);
            }
        }

        public Action<ReadonlyFeatureWrapper> OnFeatureChecked { get; set; }


        private byte[] _featureData;
        //private AimFeature _feature;
        public AimFeature Feature
        {
            get => FormatterUtil.ObjectFromBytes<AimFeature>(_featureData);
            set => _featureData = FormatterUtil.CompressMaximumObjectToBytes(value);
        }

        private readonly Dictionary<String, String> _propertyValues=new Dictionary<string, string>();

        private ISet<string> _descriptionList;

        public IEnumerable<String> DescriptionList
        {
            get
            {
                if (_descriptionList == null)
                {
                    _descriptionList=new SortedSet<string>();

                    var enumerator = HumanReadableConverter.EnumDescriptions(Feature==null?null:Feature.Feature);
                    while (enumerator.MoveNext())
                    {
                        var item = enumerator.Current;
                        if (!string.IsNullOrWhiteSpace(item))
                        {
                            _descriptionList.Add(item);
                        }
                    }
                }
                return _descriptionList;
            }
        }

        private byte[] _xml;


        public string Xml
        {
            get => _xml == null ? null : FormatterUtil.ObjectFromBytes<string>(_xml);
            set => _xml = value == null ? null : FormatterUtil.CompressMaximumObjectToBytes(value);
        }

        public LightFeature LightFeature { get; set; }

        public void SetProperty(string property, string value)
        {
            _propertyValues[property] = value;
        }

        private void LoadProperties(AimFeature internalFeature)
        {
            if (internalFeature?.Feature == null) return;

          
            var aimObject = internalFeature.Feature as IAimObject;
            var aimPropInfoArr = AimMetadata.GetAimPropInfos(aimObject);


            var conf = DataProvider.PublicationConfiguration;
            var featureConf = conf == null ||
                !conf.FeatureConfigurations.ContainsKey((int)internalFeature.Feature.FeatureType) ? null :
                conf.FeatureConfigurations[(int)internalFeature.Feature.FeatureType];
          

            foreach (var prop in internalFeature.Feature.GetType().GetProperties())
            {
                var value = prop.GetValue(internalFeature.Feature, null);

                var propInfo = aimPropInfoArr.FirstOrDefault(t => t.Name == prop.Name);

                if (propInfo==null)
                {
                    _propertyValues[prop.Name] = HumanReadableConverter.ToHuman(value);
                }
                else
                {
                    var propConf = featureConf == null ||
                       featureConf.ObjectConfiguration == null ||
                       !featureConf.ObjectConfiguration.Properties.ContainsKey(propInfo.Index)
                       ? null
                       : featureConf.ObjectConfiguration.Properties[propInfo.Index];
                    _propertyValues[prop.Name] = HumanReadableConverter.ToHuman(value, propConf);
                }
            }

          

            foreach (var aimPropInfo in aimPropInfoArr)
            {
                var reason = internalFeature.Feature.GetNilReason(aimPropInfo.Index);
                if (reason != null)
                {
                    _propertyValues[aimPropInfo.Name] = "NULL (" + reason + ")";
                }
            }
        }

        public ReadonlyFeatureWrapper(AimFeature state)
        {
            //init state
            Feature = state;
            Identifier = state.Identifier;
            //load descriptions
            //DescriptionList = HumanReadableConverter.DescriptionList(Feature.Feature);

            //load properties
            LoadProperties(state);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            string s;
            if (_propertyValues.TryGetValue(binder.Name,out s))
            {
                result = s;
                return true;
            }
            result = null;
            return false;
        }

        public string GetProperty(string property)
        {
            string s;
            _propertyValues.TryGetValue(property, out s);
            return s;
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            // Converting the property name to lowercase
            // so that property names become case-insensitive.
            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }


        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string msg = "Invalid property name: " + propertyName;

                if (this.ThrowOnInvalidPropertyName)
                    throw new Exception(msg);
                else
                    Debug.Fail(msg);
            }
        }


        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _propertyValues.Keys;
        }
        /// <summary>
        /// Returns whether an exception is thrown, or if a Debug.Fail() is used
        /// when an invalid property name is passed to the VerifyPropertyName method.
        /// The default value is false, but subclasses used by unit tests might 
        /// override this property's getter to return true.
        /// </summary>
        protected virtual bool ThrowOnInvalidPropertyName { get; private set; }

        /// <summary>
        /// Raised when a property on this object has a new value.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises this object's PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property that has a new value.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            PropertyChangedEventHandler handler = this.PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
