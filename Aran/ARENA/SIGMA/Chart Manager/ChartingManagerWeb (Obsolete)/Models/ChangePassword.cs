using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class ChangePassword
    {
        public void ChangePasswrd(ChartManagerServiceClient con, long id, string oldpsw, string newpsw)
        {
            try
            {
                con.ChangePassword(id, oldpsw, newpsw);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public void OnlyAdminChangePsw(ChartManagerServiceClient con, string OnlyAdmin, string oldpsw, string newpsw)
        {
            List<ChartUser> user = new List<ChartUser>();
            user = con.GetAllUser().Where(admn => admn.UserName == "admin").ToList();
            long id = user[0].Id;
            con.ChangePassword(id, oldpsw, newpsw);
        }
    }
}