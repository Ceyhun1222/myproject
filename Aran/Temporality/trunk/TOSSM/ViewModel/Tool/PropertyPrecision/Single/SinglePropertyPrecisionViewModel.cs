using Aran.Aim;
using TOSSM.ViewModel.Document.Single.Editable;
using TOSSM.ViewModel.Document.Single.ReadOnly;
using TOSSM.ViewModel.Tool.PropertyPrecision.Editor;

namespace TOSSM.ViewModel.Tool.PropertyPrecision.Single
{
    public class SinglePropertyPrecisionViewModel : SingleModelBase
    {
        private bool _isChangedByChildren;
        private AimPropInfo _propInfo;
       
        public bool IsChangedByChildren
        {
            get => _isChangedByChildren;
            set => _isChangedByChildren = value;
        }

        public string PropertyDescription { get; set; }

        public AimPropInfo PropInfo
        {
            get => _propInfo;
            set
            {
                _propInfo = value;
                if (PropInfo == null) return;

                PropertyName = PropInfo.Name;

                PropertyDescription = CustomPropertyToolTip.CustomToolTips.ContainsKey(PropInfo.Name) ?
                    CustomPropertyToolTip.CustomToolTips[PropInfo.Name] :
                    string.IsNullOrEmpty(PropInfo.Documentation) ? PropInfo.Name : PropInfo.Documentation;


            }
        }

		public PrecisionSubEditorViewModel ParentModel { get; set; }
    }
}
