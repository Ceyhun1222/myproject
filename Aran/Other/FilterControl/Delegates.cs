using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aran.Aim;

namespace Aran.Controls
{
    #region EventArgs

    public class PropertySelectedEventArgs : EventArgs
    {
        public PropertySelectedEventArgs (AimPropInfo [] selectedProp)
        {
            SelectedProp = selectedProp;
            Cancel = false;
        }

        public AimPropInfo [] SelectedProp { get; private set; }

        public bool Cancel { get; set; }
    }

    public class SelectorAddedPropInfoEventArgs : EventArgs
    {
        public SelectorAddedPropInfoEventArgs (AimPropInfo propInfo)
        {
            PropInfo = propInfo;
            AddToList = false;
        }

        public AimPropInfo PropInfo { get; private set; }

        public bool AddToList { get; set; }
    }


    #endregion

    #region Delegates

    public delegate void PropertySelectedEventHandler (object sender, PropertySelectedEventArgs e);
    public delegate void SelectorAddedPropInfoEventHandler (object sender, SelectorAddedPropInfoEventArgs e);

    #endregion
}
