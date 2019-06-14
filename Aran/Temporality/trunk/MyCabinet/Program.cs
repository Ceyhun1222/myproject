using System;
using Aran.Temporality.CommonUtil.Util;
using MyAccount.View;

namespace MyAccount
{
    class Program
    {
        [STAThread]
        static void Main()
        {

            var app = new SecuredApplication
                          {
                              MainAction = () => new MyAccountWindow().ShowDialog()
                          };
            app.Run();
        }
    }
}
