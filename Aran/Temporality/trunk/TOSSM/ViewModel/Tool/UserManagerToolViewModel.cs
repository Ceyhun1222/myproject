using System;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using TOSSM.ViewModel.Pane.Base;

namespace TOSSM.ViewModel.Tool
{
    public class UserManagerToolViewModel : ToolViewModel
    {
         #region Ctor

        public static string ToolContentId = "User Manager";

        public override Uri IconSource => new Uri("pack://application:,,,/Resources/Images/user.png", UriKind.RelativeOrAbsolute);

        public UserManagerToolViewModel() : base(ToolContentId)
        {
            ContentId = ToolContentId;
        }

        protected override bool Enable()
        {
            return (CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.SuperAdmin) != 0;
        }

        #endregion


    }
}
