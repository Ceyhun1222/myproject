using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class LoadHomePageList
    {
        public List<ChartUser> UserList(ChartManagerServiceClient conn)
        {
            List<ChartUser> datatable = new List<ChartUser>();
            datatable = conn.GetAllUser().Where(admin => admin.UserName != "admin").ToList();
            return datatable;
        }
    }
}