using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries.Operators;
using Aran.Temporality.Common.Aim.Extension.Property;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.ArcGis;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Helper;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Common.Util.TypeUtil;
using Aran.Temporality.Internal.MetaData;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.esriSystem;
using Point = Aran.Geometries.Point;
using Polygon = Aran.Geometries.Polygon;
using Ring = Aran.Geometries.Ring;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;
using Aran.Temporality.Common.Entity.Util;

namespace Temporality.Test
{
    class Program
    {
        

        static readonly Guid TestGuid = new Guid("8f2ccba5-5056-486d-b0c7-6d540df86cdb");
        static readonly Guid TestGuid2 = new Guid("9f2ccba5-5056-486d-b0c7-6d540df86cdb");


   

        //public static void AimTest()
        //{
        //    var s = new Stopwatch();

        //    bool needInit = false;

        //    using (var storage = AimServiceFactory.CreateLocal("Aim"))
        //    {
        //        Console.WriteLine();
        //        var provider = PrepareProvider();
                
        //        if (needInit)
        //        {
        //            storage.Truncate();

        //            foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
        //            {
        //                //get data from provider
        //                var result = provider.GetVersionsOf((FeatureType)featureType, TimeSliceInterpretationType.BASELINE);
        //                if (result.IsSucceed)
        //                {
        //                    if (result.List.Count > 0)
        //                    {
        //                        Console.WriteLine("loaded " + result.List.Count + " of " + featureType);

        //                        Guid g = Guid.NewGuid();

        //                        FeatureId featureMask;

        //                        foreach (var data in result.List)
        //                        {
        //                            var feature = data as Feature;
        //                            g = feature.Identifier;

        //                            featureMask = new FeatureId()
        //                            {
        //                                Guid = g,
        //                                FeatureTypeId = (int)featureType
        //                            };

        //                            storage.CommitEvent(new AimEvent
        //                            {
        //                                Interpretation = Interpretation.PermanentDelta,
        //                                Version = new TimeSliceVersion(feature.TimeSlice.SequenceNumber, feature.TimeSlice.CorrectionNumber),
        //                                TimeSlice = new TimeSlice(feature.TimeSlice.ValidTime.BeginPosition, feature.TimeSlice.ValidTime.EndPosition),
        //                                Data = feature,
        //                            });

        //                            //var res = storage.GetSnapshot(featureMask, DateTime.Now.AddDays(1));
        //                        }

        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("can not load " + featureType);
        //                }

        //            }
        //        }

        //        var r = provider.GetVersionsOf(FeatureType.AirportHeliport, TimeSliceInterpretationType.BASELINE);
                    

        //        foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
        //        {
        //            var result = provider.GetVersionsOf((FeatureType) featureType, TimeSliceInterpretationType.BASELINE);
        //            if (result.IsSucceed)
        //            {
        //                if (result.List.Count > 0)
        //                {
        //                    var featureMask = new FeatureId()
        //                                          {
        //                                              FeatureTypeId = (int) featureType
        //                                          };

        //                    storage.CommitNewEvent(new AimEvent
        //                                               {
        //                                                   FeatureTypeId = (int)featureType,
        //                                                   Guid = ((Feature)result.List[0]).Identifier,
        //                                                   Interpretation = Interpretation.PermanentDelta,
        //                                                   TimeSlice = new TimeSlice(new DateTime(1990,1,1)),
        //                                                   Data = AimObjectFactory.CreateFeature((FeatureType) featureType)
        //                                               });

        //                    s.Start();
        //                    var list = storage.GetActualDataByDate(Interpretation.BaseLine, featureMask, null, DateTime.Now.AddDays(1));

        //                    //Debug.Assert(list.Count == result.List.Count);
        //                    s.Stop();
        //                    var total = list.Count;
        //                    if (total > 0)
        //                    {
        //                        System.Console.WriteLine("Get " + total + " " + featureType + ": " +
        //                                                 s.ElapsedMilliseconds
        //                                                 + "ms ( " + s.ElapsedMilliseconds/total + " per snapshot )");
        //                    }
        //                }
        //            }
        //        }

        //        while (storage.Optimize())
        //        {
        //        }
        //    }
        //}

        ////public static void PerformanceTest()
        ////{

        ////    bool needInit = true;

        ////    var random = new Random();
        ////    const int totalDates = 100;
        ////    const int totalFeatures = 100;
        ////    const int totalSequences = 100;


        ////    var ids = new List<Guid>();
        ////    ids.Add(TestGuid);
        ////    for (var i = 0; i < totalFeatures - 1; i++)
        ////    {
        ////        ids.Add(Guid.NewGuid());
        ////    }



        ////    var s = new Stopwatch();
        ////    s.Start();

        ////    var now = new DateTime(2014, 1, 1);

        ////    //init our storage
        ////    using (var storage = DummyStorageFactory.CreateInstance(Path + "\\Dummy"))
        ////    {
        ////        //init dates
        ////        var dates = new List<DateTime>(totalDates);
        ////        for (var i = 0; i < totalDates; i++)
        ////        {
        ////            dates.Add(now.AddDays(i + 1));
        ////        }


        ////        if (needInit)
        ////        {



        ////            storage.Truncate();
        ////            s.Stop();
        ////            System.Console.WriteLine("Truncation: " + s.ElapsedMilliseconds + "ms");

        ////            s.Reset();
        ////            s.Start();

        ////            //write data
        ////            for (var featureCounter = 0; featureCounter < totalFeatures; featureCounter++)
        ////            {
        ////                //if (featureCounter % 10 == 0) System.Console.WriteLine(featureCounter);

        ////                for (var sequenceCounter = 1; sequenceCounter < totalSequences + 1; sequenceCounter++)
        ////                {
        ////                        storage.CommitEvent(new DummyEvent()
        ////                        {
        ////                            Interpretation = Interpretation.PermanentDelta,
        ////                            Version = new TimeSliceVersion(sequenceCounter, 0),
        ////                            TimeSlice = new TimeSlice(dates[1 + random.Next(totalDates - 2)]),//from random date after creation
        ////                            Data = new DummySubFeature()
        ////                            {
        ////                                Id = ids[featureCounter],
        ////                                PropertyA = "Value " + random.Next()//set random value
        ////                            }
        ////                        });


        ////                }
        ////            }

        ////            s.Stop();
        ////            System.Console.WriteLine("Initialization: " + s.ElapsedMilliseconds
        ////                + "ms (" + s.ElapsedMilliseconds / (totalFeatures) + " ms per state, " +
        ////                s.ElapsedMilliseconds / (totalFeatures * totalSequences) +
        ////                                     " ms per event)");

        ////            s.Reset();
        ////            s.Start();
        ////        }

        ////        System.Console.WriteLine("Features: " + totalFeatures);
        ////        System.Console.WriteLine("Sequences: " + totalSequences);
        ////        System.Console.WriteLine("Events: " + totalFeatures * totalSequences);




        ////        storage.CommitNewEvent(new DummyEvent()
        ////        {
        ////            Interpretation = Interpretation.PermanentDelta,
        ////            TimeSlice = new TimeSlice(dates[1]),//right after creation
        ////            Data = new DummySubFeature()
        ////            {
        ////                Id = TestGuid,
        ////                PropertyA = "Value " + random.Next()//set random value
        ////            }
        ////        });
        ////        s.Stop();
        ////        System.Console.WriteLine("Sending single event: " + s.ElapsedMilliseconds + "ms");
        ////        s.Restart();


        ////        //storage.Optimize();
        ////        s.Stop();
        ////        //System.Console.WriteLine("Optimization: " + s.ElapsedMilliseconds + "ms");
        ////        s.Restart();

        ////        //var result=storage.QueryState(new AbstractPredicateQuery<DummySubFeature>(
        ////        //   t => t.PropertyA != null && t.PropertyA.StartsWith("Val")), dates[totalDates-1]);//heaviest query - should retreive whole db

        ////        //Debug.Assert(result.Count()==totalFeatures);

        ////        ////just in case enum all
        ////        //foreach (var state in result.Results)
        ////        //{

        ////        //}

        ////        //s.Stop();
        ////        //System.Console.WriteLine("Whole db query (" +result.Count() +
        ////        //                         " results): " + s.ElapsedMilliseconds + "ms (" + s.ElapsedMilliseconds / result.Count()+" ms per item)");
        ////        //s.Reset();
        ////        //s.Start();

        ////        //var result1 = storage.QueryState(new AbstractPredicateQuery<DummySubFeature>(
        ////        //   t => t.PropertyA != null && t.PropertyA.EndsWith("1")), dates[totalDates - 1]);//heaviest query - should retreive whole db



        ////        //s.Stop();
        ////        //System.Console.WriteLine("Pattern db query (" + result1.Count() +
        ////        //                         " results) : " + s.ElapsedMilliseconds + "ms (" + s.ElapsedMilliseconds / result1.Count() + " ms per item)");
        ////        s.Reset();



        ////        var featureMask = new FeatureId()
        ////        {
        ////            Guid = TestGuid,
        ////            FeatureTypeId = TypeUtil.GetFeatureType(typeof(DummySubFeature))
        ////        };
        ////        s.Start();
        ////        var total = 1;
        ////        for (int i = 0; i < total; i++)
        ////        {
        ////            var result2 = storage.GetSnapshot(featureMask, dates[totalDates - 1]);
        ////        }
        ////        s.Stop();

        ////        //Debug.Assert(result2!=null);


        ////        System.Console.WriteLine("Get " + total +
        ////                                 " snapshots: " + s.ElapsedMilliseconds + "ms ( " + s.ElapsedMilliseconds / total + " per snapshot )");

        ////        s.Restart();
        ////        IList<AbstractState<DummyFeature>> list = storage.GetSnapshotByFeatureName(featureMask, dates[totalDates - 1]);
        ////        s.Stop();
        ////        System.Console.WriteLine("Get " + list.Count +
        ////                               " snapshots: " + s.ElapsedMilliseconds + "ms ( " + s.ElapsedMilliseconds / list.Count + " per snapshot )");


        ////        long memory = GC.GetTotalMemory(true) / (1024 * 1024);
        ////        Console.WriteLine("Memory " + memory + " mb");





        ////        s.Restart();
        ////        IList<AbstractState<DummyFeature>> list2 = storage.GetSnapshotByFeatureName(featureMask, dates[totalDates - 1]);
        ////        s.Stop();
        ////        System.Console.WriteLine("Get second " + list.Count +
        ////                               " snapshots: " + s.ElapsedMilliseconds + "ms ( " + s.ElapsedMilliseconds / list.Count + " per snapshot )");


        ////        s.Restart();
        ////        s.Stop();
        ////        //System.Console.WriteLine("Last optimization: " + s.ElapsedMilliseconds + "ms");
        ////        s.Restart();


        ////        while (storage.Optimize());


        ////        //check memory here
        ////        memory = GC.GetTotalMemory(true) / (1024 * 1024);//in mb

        ////        var currentProcess = Process.GetCurrentProcess();
        ////        long physicalMem = currentProcess.WorkingSet64 / (1024 * 1024);//in mb
        ////        long privateMem = currentProcess.PrivateMemorySize64 / (1024 * 1024);//in mb
        ////        //
        ////    }
        ////}


        //static void CommitTest1()
        //{
            
        //    try
        //    {
                
        //        using(var temporalityService = AimServiceFactory.CreateLocal("Test"))
        //        {
                   
        //            var service = new ServiceHelper(temporalityService);

        //            service.Storage.Truncate();
        //            var wp = service.WorkPackage.CreateNewWorkPackage(true,"test safe wp");


        //            var data = new AirportHeliport()
        //                           {
        //                               Identifier = TestGuid,
        //                               Name = "ASAD"
        //                           };

                   

        //            var data2 = new Runway
        //                            {
        //                                Identifier = TestGuid2,
        //                                AssociatedAirportHeliport = new FeatureRef(TestGuid),
        //                            };


        //            var myEvent = new AimEvent
        //            {
        //                WorkPackage = wp,
        //                Interpretation = Interpretation.PermanentDelta,
        //                TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
        //                LifeTimeBegin = new DateTime(1991, 1, 1),
        //                Data = data
        //            };

        //            var myEvent2 = new AimEvent
        //            {
        //                WorkPackage = wp,
        //                Interpretation = Interpretation.PermanentDelta,
        //                TimeSlice = new TimeSlice(new DateTime(1992, 1, 1)),
        //                LifeTimeBegin = new DateTime(1992, 1, 1),
        //                Data = data2
        //            };


        //            var result = service.Event.CommitNewEvent(myEvent);
        //            var result2 = service.Event.CommitNewEvent(myEvent2);

        //            var mask = new FeatureId()
        //            {
        //                FeatureTypeId = (int)FeatureType.Runway,
        //                Guid = TestGuid2,
        //                WorkPackage = wp
        //            };


        //            var list = service.State.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);
        //            var item = ((list[0] as AimState).Data as AirportHeliport);
        //            var name = item.Name;


        //            //service.WorkPackage.CommitWorkPackage(wp);

        //            service.WorkPackage.RollbackWorkPackage(wp);

        //            list = service.State.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);

        //            mask.WorkPackage = 0;

        //            list = service.State.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);
                    
                    
                    
        //        }
                
                
        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }



        //}

       

        //static void ReadTest2()
        //{
        //    //var airspace = new Airspace();
        //    //airspace.GeometryComponent.Add(new AirspaceGeometryComponent());
        //    //airspace.GeometryComponent[0].TheAirspaceVolume = new AirspaceVolume();
        //    //airspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection = new Surface();
            
        //    ////does not work
        //    //airspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo.Centroid.X=10;
        //    //var x = airspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo.Centroid.X;
        //    ////does not work
        //    //airspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo.Centroid.SetCoords(10,10);

        //    //var p = new Polygon();
        //    //p.ExteriorRing=new Ring();
        //    //p.ExteriorRing.Add(new Point(10,10));
        //    //p.ExteriorRing.Add(new Point(10, 20));
        //    //p.ExteriorRing.Add(new Point(20, 20));
        //    //p.ExteriorRing.Add(new Point(20, 10));

        //    //airspace.GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo.Add(p);

        //    //var clone = FormatterUtil.ObjectFromBytes<Airspace>(FormatterUtil.ObjectToBytes(airspace));

        //    try
        //    {

        //        using (var temporalityService = AimServiceFactory.CreateLocal("Map"))
        //        {

        //            var service = new ServiceHelper(temporalityService);

        //            var mask = new FeatureId()
        //            {
        //                FeatureTypeId = (int)FeatureType.Airspace,
        //                Guid = new Guid("ee673a87-721d-48b4-8e20-359fc038a32a")
        //            };


        //            var list = service.State.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);
        //           var airspace = list[0].Data as Airspace;
        //            //var c=(list[0].Data as Airspace).GeometryComponent[0].TheAirspaceVolume.HorizontalProjection.Geo.Centroid;

        //            var bytes = FormatterUtil.ObjectToBytes(airspace);
        //            var clone = FormatterUtil.ObjectFromBytes<Airspace>(bytes);


        //        }


        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }



        //}

        //public static void ClientServerTest()
        //{


        //    try
        //    {
        //        TemporalityServer.StartServer();

        //        //should be ok
        //        using (var remoteStorage1 = AimServiceFactory.CreateRemote("ClientServerTest", "user", "pass"))
        //        {
        //            remoteStorage1.Truncate();

        //            var time1 = remoteStorage1.GetServerTime();
        //            Console.WriteLine(@"Server time is " + time1);

        //            remoteStorage1.CommitNewEvent(new AimEvent
        //            {
        //                FeatureTypeId = (int)FeatureType.AirportHeliport,
        //                Guid = Guid.NewGuid(),
        //                Interpretation = Interpretation.PermanentDelta,
        //                TimeSlice = new TimeSlice(new DateTime(1990, 1, 1)),
        //                Data = AimObjectFactory.CreateFeature(FeatureType.AirportHeliport)
        //            });

        //            var list = remoteStorage1.GetActualDataByDate(Interpretation.BaseLine,
        //                new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport }, null, DateTime.Now);
        //        }


        //        //should cause authentification exception, only "user" and "pass" are allowed
        //        using (var remoteStorage = AimServiceFactory.CreateRemote("ClientServerTest", "user1", "pass"))
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

        //public static void RemoteTest()
        //{


        //    try
        //    {

        //        //should be ok
        //        using (var remoteStorage1 = AimServiceFactory.CreateRemote("Test", "user", "pass","172.30.31.18:8523"))
        //        {
        //            var time1 = remoteStorage1.GetServerTime();
        //            Console.WriteLine(@"Server time is " + time1);

        //            remoteStorage1.Truncate();

        //            var d=AimObjectFactory.CreateFeature(FeatureType.AirportHeliport);
                   
        //            var res=remoteStorage1.CommitNewEvent(new AimEvent
        //            {
        //                FeatureTypeId = (int)FeatureType.AirportHeliport,
        //                Guid = Guid.NewGuid(),
        //                Interpretation = Interpretation.PermanentDelta,
        //                TimeSlice = new TimeSlice(new DateTime(1990, 1, 1)),
        //                Data =d
        //            });

        //            var list = remoteStorage1.GetActualDataByDate(Interpretation.BaseLine,
        //                new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport }, null, DateTime.Now);
        //        }


        //        ////should cause authentification exception, only "user" and "pass" are allowed
        //        //using (var remoteStorage = AimServiceFactory.CreateRemote("ClientServerTest", "user1", "pass"))
        //        //{
        //        //    var time = remoteStorage.GetServerTime();
        //        //    Console.WriteLine(@"Server time is " + time);
        //        //}

        //    }
        //    catch (Exception exception)
        //    {
        //        Console.WriteLine(exception.Message);
        //    }
        //    finally
        //    {
        //    }
        //}

        static void CreateStorageFromXml(string storageName, string xmlPath)
        {
            var provider = DbProviderFactory.Create("Aran.Aim.Data.XmlProvider");
            provider.DefaultEffectiveDate = DateTime.Now;
            provider.Open(xmlPath);

            //using (var storage = AimServiceFactory.CreateRemote(storageName,"user","password"))
            //using (var storage = AimServiceFactory.CreateRemote(storageName, "user", "password", "172.30.31.18:8523"))
            using (var storage = AimServiceFactory.OpenLocal(storageName))
            {
                storage.Truncate();
                var wp = 0;// storage.CreateNewWorkPackage();

                foreach (var featureType in Enum.GetValues(typeof(FeatureType)))
                {
                    //get data from provider
                    var result = provider.GetVersionsOf((FeatureType)featureType, TimeSliceInterpretationType.BASELINE);
                    if (result.IsSucceed)
                    {
                        if (result.List.Count > 0)
                        {
                            Console.WriteLine("loaded " + result.List.Count + " of " + featureType);

                            foreach (var data in result.List)
                            {
                                //Console.Write(".");
                                var feature = data as Feature;
                                var myEvent = new AimEvent
                                {
                                    WorkPackage = wp,
                                    Interpretation = Interpretation.PermanentDelta,
                                    Version =
                                        new TimeSliceVersion(feature.TimeSlice.SequenceNumber,
                                                             feature.TimeSlice.CorrectionNumber),
                                    TimeSlice =
                                        new TimeSlice(feature.TimeSlice.ValidTime.BeginPosition),
                                    Data = feature,
                                };

                                myEvent.Data.InitEsriExtension();

                                var counter = 10;
                                while (counter-->0)
                                {
                                    try
                                    {
                                        var result2 = storage.CommitNewEvent(myEvent);
                                        if (!result2.IsOk)
                                        {
                                            Console.WriteLine(result2.ErrorMessage);
                                        }
                                        break;
                                    }
                                    catch (Exception exception)
                                    {
                                        Console.WriteLine(exception.Message);
                                    }
                                }
                                
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("can not load " + featureType);
                    }
                }

            }
        }

        //static void ArcGisTest()
        //{

        //    var operators = new GeometryOperators();
        //    var polygon = new Polygon
        //    {
        //        ExteriorRing =
        //            new Ring { new Point(40, 40), new Point(60, 40), new Point(60, 60), new Point(40, 60) }
        //    };
        //    var polygon2 = new Polygon
        //    {
        //        ExteriorRing =
        //            new Ring { new Point(40, 40), new Point(60, 40), new Point(60, 60), new Point(40, 60) }
        //    };

        //    var point = new Point(50, 50);
        //    //var result=operators.Contains(polygon, point);


        //    for (int i = 0; i < 400; i++)
        //    {
        //        var result = operators.Intersect(polygon, polygon2);
        //        //IRelationalOperator relOper = (IRelationalOperator)esriGeom1;


        //        //var result = relOper.Contains(esriGeom2);
        //    }


        //    //IGeometry esriGeom1 = ConvertToEsriGeom.FromGeometry(polygon);
        //    //IGeometry esriGeom2 = ConvertToEsriGeom.FromGeometry(point);
        //}

        static void MySerializationTest()
        {
            var meta = new EventMetaData{WorkPackage = 10};
            var bytes1 = FormatterUtil.ObjectToBytes(meta);
            var e1 = FormatterUtil.ObjectFromBytes<EventMetaData>(bytes1);

            var e = new AimEvent
                      {
                          WorkPackage = 10,
                          Data = new AirportHeliport() ,
                      };

            e.Data.PropertyExtensions.Add(new EsriPropertyExtension{PropertyIndex = 1});
            var bytes = FormatterUtil.ObjectToBytes(e);
            var e2 = FormatterUtil.ObjectFromBytes<AimEvent>(bytes);
        }


        static void ReadTest1()
        {

            try
            {

                using (var temporalityService = AimServiceFactory.OpenLocal("hesh"))
                {

                 
                    var mask = new FeatureId()
                    {
                        FeatureTypeId = (int)FeatureType.AirportHeliport,
                    };


                    var list = temporalityService.GetActualDataByDate(mask, false, DateTime.Now);

                }


            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }



        }

    
        static void Main(string[] args)
        {
			//var l = new LightFeature
			//{
			//	Fields = new[]{
			//	new LightField{
			//		Name="Test",
			//		Value=new ValSpeed(10,UomSpeed.KM_H)}}
			//};

            //LightField lghtField = new LightField
            //{
            //    Name = "Test",
            //    Value = new ValSpeed ( 10, UomSpeed.KM_H ),
            //};

            //var bytes = FormatterUtil.ObjectToBytes ( lghtField );
            //var clone = FormatterUtil.ObjectFromBytes<LightField> ( bytes );
            //Console.WriteLine ( clone.Name );
            //Console.WriteLine ( ( ( ValSpeed ) clone.Value ).Value );
            //Console.WriteLine ( ( ( ValSpeed ) clone.Value ).Uom );
            //Console.Read ( );
            //return;



            //MySerializationTest();
            //return;

            //ESRI License Initializer generated code.
            LicenseInitializer.Instance.InitializeApplication(
                new esriLicenseProductCode[] { esriLicenseProductCode.esriLicenseProductCodeBasic },
                new esriLicenseExtensionCode[] { });


            TypeUtil.CurrentTypeUtil = new AimTypeUtil();
            //log4net.Config.XmlConfigurator.Configure();

            //uncomment to crete default tables
            AimServiceFactory.Setup();

            //CreateStorageFromXml("latvia2", @"D:\latvian-data1.xml");

            //CreateStorageFromXml("hesh", @"D:\hesh-aixm-5.1.xml");/`
            //CreateStorageFromXml("peru", @" D:\snapshots\2014-08-26.aixm5.xml");


            ReadTest1();

            //CreateStorageFromXml("Map", @"D:\AtsRoute\AtsRoute.xml");
            //CreateStorageFromXml("Map", @"D:\test.xml");

            //CreateStorageFromXml("Map", @"D:\latvia.xml");
            //CreateStorageFromXml("Map", @"D:\lat_test.xml");



           // ClientServerTest();


            //MySerializationTest();

            //RemoteTest();
            //CommitTest1();
           



            //ReadTest2();


            //var s = new Stopwatch();
            //s.Start();






            //PerformanceTest();
            //AimTest();

            //s.Stop();
            //Console.WriteLine("Total elapsed time: " + s.ElapsedMilliseconds + "ms");


            //ESRI License Initializer generated code.
            //Do not make any call to ArcObjects after ShutDownApplication()
            LicenseInitializer.Instance.ShutdownApplication();
        } 
    }
}
