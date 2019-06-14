using System;
using System.Windows;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.CommonUtil.Context;
using Aran.Temporality.CommonUtil.Util;
using Aran.Temporality.CommonUtil.View;

namespace UserManager
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            var app = new SecuredApplication
                          {
                              MainAction = () =>
                                               {
                                                   if (CurrentDataContext.CurrentUser==null|| 
                                                       (CurrentDataContext.CurrentUser.RoleFlag & (int)UserRole.SuperAdmin)==0)
                                                   {
                                                       MessageBox.Show(
                                                           "Sorry, you do not have permission to run this app.",
                                                           "Access denied", MessageBoxButton.OK, MessageBoxImage.Stop);
                                                   }
                                                   else
                                                   {
                                                       new EasyManagementWindow().ShowDialog();
                                                   }
                                               }
                          };
            app.Run();
        }
    }
}
