using System;
using AIMSLServiceClient.Services;
using AIMSLServiceClient.Config;

namespace AIMSLServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                AimslService.Init();
                var service = AimslService.GetService();
                var list = service.GetAllSubscriptions();

                //var topics = service.GetAllTopics("*");
                //var download = service.Download(".");
                //var upload = service.Upload("D:\\Export.xml");
                //var res = service.CreatePullPoint();
                //Console.WriteLine(res);
                //var sub = service.Subscribe(res, upload.Uuid);
                //var message = service.GetMessages(res.Split('=')[1]);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }
    }
}
