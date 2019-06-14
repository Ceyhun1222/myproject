using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text;
using ChartManager.Tests.ServiceReference1;
using Ploeh.AutoFixture;
using System.Security.Cryptography;
using System.ServiceModel;

namespace ChartManager.Tests
{
    public class MyCallbackClient : IChartManagerServiceCallback
    {
        public void ChartChanged(Chart chart, ChartCallBackType type)
        {
            Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} is called");
        }

        public void AllChartVersionsDeleted(Guid identifier)
        {
            Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} is called");
        }

        public void ChartsByEffectiveDateDeleted(Guid identifier, DateTime dateTime)
        {
            Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} is called");
        }

        public void UserChanged(UserCallbackType type)
        {
            Console.WriteLine($"{System.Reflection.MethodBase.GetCurrentMethod().Name} is called");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            
            Fixture fixture = new Fixture();
            fixture.Customizations.Add(new RandomEnumSequenceGenerator());
            fixture.Customizations.Add(new PropertyTypeOmitter(typeof(ExtensionDataObject)));
            var uri = new Uri("net.tcp://localhost");
            var callback = new MyCallbackClient();
            var instanceContext = new InstanceContext(callback);
            ChartManagerServiceClient client = new ChartManagerServiceClient(instanceContext, "netTcp_ChartService");

#warning Чтение из config файла.
            //var registry = new ModifyRegistry
            //{
            //    ShowError = false,
            //    BaseRegistryKey = Registry.CurrentUser,
            //    SubKey = createdConfigPath
            //};        
            
            client.ClientCredentials.UserName.UserName = "admin";//registry.Read("Username");
            client.ClientCredentials.UserName.Password = "d77747d67333bd4fe0bcba500abd64b16ea08cb6b41e3b5a7b644e57ddcfc42a";//registry.Read("Password");

            //ChartData chart = new ChartData()
            //{
            //    BeginEffectiveDate = DateTime.Now,
            //    Airport = "Arp_",
            //    Identifier = Guid.NewGuid(),
                
            //    Organization = "Org_",
            //    RunwayDirection = "RwyDir_",
            //    Type = ChartType.Aerodrome,
                
            //};

            //client.Upload(chart);

            //client.ChangePassword(4, "admin", "admin");

            //var config = client.GetConfigFile(1);

            //client.Close();

#warning Создание config файла на стороне клиента
            //var doc = new XmlDocument();
            //doc.LoadXml(
            //    "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n" +
            //    "<Config>\n" +
            //    "</Config>");

            //XmlElement elem = doc.DocumentElement;

            //var userName = doc.CreateElement("Username");
            //userName.InnerText = config.UserName;
            //elem.AppendChild(userName);

            //var pass = doc.CreateElement("Password");
            //pass.InnerText = config.Password;
            //elem.AppendChild(pass);

            //var addressTcp = doc.CreateElement("AddressTCP");
            //addressTcp.InnerText = config.AddressTcp;
            //elem.AppendChild(addressTcp);

            //var addressHttp = doc.CreateElement("AddressHTTP");
            //addressHttp.InnerText = config.AddressHttp;
            //elem.AppendChild(addressHttp);

            //doc.Save("C:\\Config.xml");

            Console.Read();
        }

        //public static string GetLocalIPAddress()
        //{
        //    var host = Dns.GetHostEntry(Dns.GetHostName());
        //    foreach (var ip in host.AddressList)
        //    {
        //        if (ip.AddressFamily == AddressFamily.InterNetwork)
        //        {
        //            return ip.ToString();
        //        }
        //    }
        //    throw new Exception("No network adapters with an IPv4 address in the system!");
        //}

        //private static void TestUpdateDownload(ChartManagerServiceClient client)
        //{
        //    var folder = @"D:\Test_Images";
        //    int maxSeed = Enum.GetValues(typeof(ChartType)).Length;
        //    Random random = new Random();
        //    Stopwatch stopwatch = new Stopwatch();
        //    Console.WriteLine("Enter count of chart to create ");
        //    while (true)
        //    {
        //        if (int.TryParse(Console.ReadLine(), out int count))
        //        {
        //            for (int i = 1; i <= count; i++)
        //            {
        //                stopwatch.Restart();
        //                ChartData chart = new ChartData()
        //                {
        //                    BeginEffectiveDate = DateTime.Now.AddMonths(i),
        //                    Airport = $"Arp_{i}",
        //                    Identifier = Guid.NewGuid(),
        //                    CreatedAt = DateTime.Now.AddMonths(-i),
        //                    Organization = $"Org_{i}",
        //                    RunwayDirection = $"RwyDir_{i}",
        //                    Type = (ChartType) random.Next(maxSeed),
        //                    Preview = ConvertImage(Image.FromFile(Path.Combine(folder, random.Next(1,maxSeed).ToString() + ".jpeg")))
        //                };
        //                //client.UploadSource(chart, false);
        //                stopwatch.Stop();
        //                Console.WriteLine($"{i} is uploaded in {stopwatch.ElapsedMilliseconds} milliseconds");
        //                Console.WriteLine($"Press any key to continue");
        //                Console.ReadLine();
        //            }
        //        }
        //        else break;
        //    }
        //}

        //private static byte[] ConvertImage(Image image)
        //{
        //    using (var ms = new MemoryStream())
        //    {
        //        image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //        return ms.ToArray();
        //    }
        //}

        //public static String Sha256_hash(String value)
        //{
        //    StringBuilder Sb = new StringBuilder();

        //    using (SHA256 hash = SHA256.Create())
        //    {
        //        Encoding enc = Encoding.UTF8;
        //        Byte[] result = hash.ComputeHash(enc.GetBytes(value));

        //        foreach (Byte b in result)
        //            Sb.Append(b.ToString("x2"));
        //    }

        //    return Sb.ToString();
        //}

        //private static void UserCreate(Fixture fixture, ChartManagerServiceClient client)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    Console.WriteLine("Enter count of user to create ");
        //    while (true)
        //    {
        //        if (int.TryParse(Console.ReadLine(), out int count))
        //        {
        //            var list = fixture.CreateMany<ChartUser>(count).ToList();


        //            stopWatch.Reset();
        //            stopWatch.Start();
        //            try
        //            {
        //                list.ForEach(client.CreateUser);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e);
        //                throw;
        //            }

        //            stopWatch.Stop();
        //            Console.WriteLine($"{count} users saved by {stopWatch.ElapsedMilliseconds} in milliseconds");
        //        }
        //        else
        //            break;
        //    }
        //}

        //private static void TestUpdate(ChartManagerServiceClient client, int id)
        //{
        //    //var user = client.GetUser(id);
        //    //while (true)
        //    //{
        //    //    user.FirstName = (2 * int.Parse(user.FirstName)).ToString();
        //    //    bool result = client.UpdateUser(user);
        //    //    Console.WriteLine("Press 'y' to continue otherwise will stop");
        //    //    if (Console.ReadKey().KeyChar != 'y')
        //    //        break;
        //    //    Console.WriteLine();
        //    //}
        //}
    }
}
