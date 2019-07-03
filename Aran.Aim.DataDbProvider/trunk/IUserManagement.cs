using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Aran.Aim.Data
{
    public interface IUserManagement
    {
        //string Open(string connectionString);
        //void Close();
        InsertingResult InsertUser(User user);
        InsertingResult UpdateUser(User user);
        InsertingResult DeleteUser(User user);
        GettingResult ReadUsers();
        GettingResult ReadUserByName(string userName);
        GettingResult ReadUsersById(long userId);
    }
}
