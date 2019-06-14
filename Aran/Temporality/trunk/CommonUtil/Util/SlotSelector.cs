using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.View;

namespace Aran.Temporality.CommonUtil.Util
{
    public class SlotSelector
    {
        public static bool ShowDialog()
        {
            var dialog = new SlotSelectorDialog();
            dialog.ShowDialog();
            return CurrentDataContext.CurrentUser.ActivePrivateSlot!=null;
        }
    }
}
