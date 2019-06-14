using Aran.Temporality.CommonUtil.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIP.BaseLib.Class
{
    public static class TossUser
    {
        public static int Id { get; set; }
        public static string Username { get; set; }
        public static int RoleFlag { get; set; }

        public static void Init()
        {
            if (CurrentDataContext.CurrentUser != null)
            {
                Id = CurrentDataContext.CurrentUser.Id;
                Username = CurrentDataContext.CurrentUser.Name;
                RoleFlag = CurrentDataContext.CurrentUser.RoleFlag;
                //CurrentDataContext.CurrentService.GetServerTime();
            }
        }
    }
}
