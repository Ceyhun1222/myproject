using Aran.Aim;
using Aran.Aim.Metadata.UI;
using Aran.Temporality.CommonUtil.Util;

namespace TOSSM.ViewModel.Control.PropertySelector.Menu
{
    public class PropertyMenuItemViewModel : MenuItemViewModel
    {
        public IChangedHandler<PropertyMenuItemViewModel> PresenterModel { get; set; }

        private AimPropInfo _propInfo;
        public AimPropInfo PropInfo
        {
            get => _propInfo;
            set
            {
                _propInfo = value;
                
                if (PropInfo==null) return;

                Header = PropInfo.Name;
                IsChecked = PropInfo.UiPropInfo().ShowGridView;
            }
        }

        public override void OnCheckedChanged()
        {
            if (PropInfo!=null)
            {
                PropInfo.UiPropInfo().ShowGridView = IsChecked;
            }


            if (PresenterModel!=null)
            {
                PresenterModel.OnChanged(this);
            }
        }

        public PropertyMenuItemViewModel(IChangedHandler<PropertyMenuItemViewModel> model): base(null)
        {
            IsCheckable = true;
            StaysOpenOnClick = true;
            PresenterModel = model;
        }


       

    }
}
