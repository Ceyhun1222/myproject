using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class DisableUser
    {
        public void DisableUsers(long id, bool disabled, ChartManagerServiceClient connect)
        {
            try
            {
                connect.DisableUser(id, disabled);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}