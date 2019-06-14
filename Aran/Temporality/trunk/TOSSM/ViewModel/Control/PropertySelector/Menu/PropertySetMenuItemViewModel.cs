using Aran.Temporality.CommonUtil.Util;

namespace TOSSM.ViewModel.Control.PropertySelector.Menu
{
    public class PropertySetMenuItemViewModel : MenuItemViewModel
    {
        public IChangedHandler<PropertySetMenuItemViewModel> PresenterModel { get; set; }

        public override void OnCheckedChanged()
        {
            if (PresenterModel == null) return;

            if (IsChecked)
            {
                PresenterModel.OnChanged(this);
            }
        }

        public PropertySetMenuItemViewModel() : base(null)
        {
            IsCheckable = true;
            StaysOpenOnClick = false;
        }
    }
}
