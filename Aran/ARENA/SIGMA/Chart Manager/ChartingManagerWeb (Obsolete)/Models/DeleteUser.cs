using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class DeleteUser
    {
        public void DeletUser(ChartManagerServiceClient conn, long id)
        {
            try
            {
                conn.DeleteUserAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}