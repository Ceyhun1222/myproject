using System;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Common.Entity.Util;
using TOSSM.Annotations;

namespace TOSSM.Util.Wrapper
{
    public class LightFeatureWrapper : DynamicObject, INotifyPropertyChanged
    {
        public readonly LightFeature LightFeature;
        public ReadonlyFeatureWrapper ReadonlyFeatureWrapper { get; set; }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public Action<LightFeatureWrapper> OnIsCheckedChanged { get; set; }
        private bool _isChecked;
       
       public bool IsChecked
        {
            get => _isChecked;
           set
            {
                _isChecked = value;
                if (OnIsCheckedChanged != null)
                {
                    OnIsCheckedChanged(this);
                }
                OnPropertyChanged("IsChecked");
            }
        }

        private bool? _isIssue;
        private string _status;

        public bool? IsIssue
        {
            get => _isIssue;
            set
            {
                _isIssue = value;
                Status = IsIssue==null? "?":IsIssue == true ? "Issue" : "Ok";
            }
        }

        public LightFeatureWrapper(LightFeature feature)
        {
            LightFeature = feature;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (ReadonlyFeatureWrapper != null && ReadonlyFeatureWrapper.TryGetMember(binder, out object res))
            {
                result = res;
                return true;
            }

            var field=(LightFeature.Fields ?? new LightField[0]).FirstOrDefault(t => t.Name == binder.Name);
            var complexField = (LightFeature.ComplexFields ?? new LightComplexField[0]).FirstOrDefault(t => t.Name == binder.Name);

            if (field == null && complexField==null)
            {
                result = null;
                return false;
            }

            if (field != null)
            {
                result = field.Value;
                return true;
            }

            result = complexField.Value;
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
