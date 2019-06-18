﻿using ChartingManagerWeb.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChartingManagerWeb.Models
{
    public class EditUser
    {
        public void Edituser(ChartManagerServiceClient connect, long id, string firstname, string lastname, string privilege, string username, string email, bool disabled, string position)
        {
            try
            {
                ChartUser chartuser = new ChartUser();
                chartuser.Id = id;
                chartuser.FirstName = firstname;
                chartuser.LastName = lastname;
                chartuser.UserName = username;
                //chartuser.Password = password;
                chartuser.Email = email;
                chartuser.Disabled = disabled;
                chartuser.Position = position;
                if (privilege == "Full")
                {
                    chartuser.Privilege = UserPrivilege.Full;
                }
                if (privilege == "ReadOnly")
                {
                    chartuser.Privilege = UserPrivilege.ReadOnly;
                }
                //string length = connect.GetAllUser().Length.ToString();
                connect.UpdateUser(chartuser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}