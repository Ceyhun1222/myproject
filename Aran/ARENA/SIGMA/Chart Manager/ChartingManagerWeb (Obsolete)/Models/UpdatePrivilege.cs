using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class UpdatePrivilege
    {
        public void ChangePrivilege(ChartManagerServiceClient connect, long id, string firstname, string lastname, string privilege, string username, string email, bool disabled, string position)
        {
            try
            {
                ChartUser chtuser = new ChartUser();
                chtuser.Id = id;
                chtuser.FirstName = firstname;
                chtuser.LastName = lastname;
                chtuser.UserName = username;
                chtuser.Email = email;
                chtuser.Disabled = disabled;
                chtuser.Position = position;
                if (privilege == "ReadOnly")
                {
                    chtuser.Privilege = UserPrivilege.ReadOnly;
                }
                if (privilege == "Full")
                {
                    chtuser.Privilege = UserPrivilege.Full;
                }
                connect.UpdateUser(chtuser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}