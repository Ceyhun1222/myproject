//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using ChartServices.DataContract;

//namespace ChartServices
//{
//    public class ChartDbContext : DbContext
//    {
//        public ChartDbContext() : base("sigma")
//        {
//#warning Temporary 
//            //admin123 = "0192023a7bbd73250516f069df18b500" in md5
//            if (Users.SingleOrDefault<User>(f =>
//                    f.IsAdmin) == null)
//                Users.Add(
//                    new User()
//                    {
//                        UserName = "admin",
//                        Password = "0192023a7bbd73250516f069df18b500",
//                        IsAdmin = true
//                    });
//        }

//        public DbSet<User> Users { get; set; }

//    }
//}
