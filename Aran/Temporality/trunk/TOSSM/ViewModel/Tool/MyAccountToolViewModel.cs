using System;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class MyAccountToolViewModel : ToolViewModel
    {
         #region Ctor

        public static string ToolContentId = "My Account";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/account.png", UriKind.RelativeOrAbsolute);

        public MyAccountToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }


        #endregion
    }
}
