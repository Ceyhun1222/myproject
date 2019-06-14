using System;
using Aran.Aim;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;

namespace Temporality.Tests
{
    [TestClass]
    public class RemoteComminicationTest
    {

        //[TestMethod]
        //public void ClientServerTest()
        //{
        //    log4net.Config.XmlConfigurator.Configure();

        //    try
        //    {
        //        TemporalityServer.StartServer();

        //        //should be ok
        //        using (var remoteStorage1 = AimServiceFactory.OpenRemote("ClientServerTest"))
        //        {
        //            remoteStorage1.Truncate();

        //            var time1 = remoteStorage1.GetServerTime();
        //            Console.WriteLine(@"Server time is " + time1);

        //            var res= remoteStorage1.CommitNewEvent(new AimEvent
        //            {
        //                FeatureTypeId = (int)FeatureType.AirportHeliport,
        //                Guid = Guid.NewGuid(),
        //                Interpretation = Interpretation.PermanentDelta,
        //                TimeSlice = new TimeSlice(new DateTime(1990, 1, 1)),
        //                Data = AimObjectFactory.CreateFeature(FeatureType.AirportHeliport)
        //            });

        //            var list = remoteStorage1.GetActualDataByDate(Interpretation.BaseLine,
        //                new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport }, 
        //                null, 
        //                DateTime.Now);
        //            Assert.IsTrue(list.Count>0);
        //        }


        //        //should cause authentification exception, only "user" and "pass" are allowed
        //        using (var remoteStorage = AimServiceFactory.OpenRemote("ClientServerTest"))
        //        {
        //            var time = remoteStorage.GetServerTime();
        //            Console.WriteLine(@"Server time is " + time);
        //        }

        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }
        //    finally
        //    {
        //        TemporalityServer.StopServer();
        //    }
        //}
    }
}
