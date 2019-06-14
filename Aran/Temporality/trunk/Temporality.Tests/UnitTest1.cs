//using System;
//using System.Diagnostics;
//using Aran.Aim;
//using Aran.Aim.Features;
//using Aran.Aim.Utilities;
//using Aran.Temporality.Aim.MetaData;
//using Aran.Temporality.Aim.Service;
//using Aran.Temporality.Common.Enum;
//using Aran.Temporality.Common.Id;
//using Aran.Temporality.Common.Interface;
//using Aran.Temporality.Common.MetaData;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Temporality.Tests
//{
//    [TestClass]
//    public class UnitTest1
//    {
//        static private readonly Guid TestGuid = new Guid("8f2ccba5-5056-486d-b0c7-6d540df86cdb");

//        [TestMethod]
//        public void CommitEvent()
//        {
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                                   Identifier = TestGuid,
//                                   Name = "ASAD"
//                               };

//                var result = temporalityService.CommitEvent(new AimEvent
//                                                        {
//                                                            Version = new TimeSliceVersion(10,10),
//                                                            Interpretation = Interpretation.PermanentDelta,
//                                                            TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                                                            Data = data
//                                                        });
//                //wrong version
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);


//                result = temporalityService.CommitEvent(new AimEvent
//                {
//                    Version = new TimeSliceVersion(10, 0),
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                //wrong version
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);
               
//            }
//        }

//        [TestMethod]
//        public void CommitEventWithNilProperty()
//        {
//            using (ITemporalityService<Feature> temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                //wrong version
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);

//                var data2 = new AirportHeliport
//                {
//                    Identifier = TestGuid,
//                };


//                data2.SetNilReason(AimMetadataUtility.GetPropertyIndexByName(data2, "Name"), NilReason.Inapplicable);

//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data2
//                });


//            }
//        }

//        [TestMethod]
//        public void CommitEventWithExistingVersion()
//        {
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };


//                var result = temporalityService.CommitEvent(new AimEvent
//                {
//                    Version = new TimeSliceVersion(1, 0),
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                //all is ok
//                Assert.IsTrue(result.IsOk);


//                result = temporalityService.CommitEvent(new AimEvent
//                {
//                    Version = new TimeSliceVersion(1, 0),
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1992, 1, 1)),
//                    Data = data
//                });
//                //wrong version
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);


//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                //same start
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);
//            }
//        }

//        [TestMethod]
//        public void CommitNewEvent()
//        {
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    Data = data
//                });
//                //no timeslice
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);

//                //result = temporalityService.CommitNewEvent(new AimEvent
//                //{
//                //    Interpretation = Interpretation.PermanentDelta,
//                //    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1), new DateTime(1999,1,1)),
//                //    Data = data
//                //});
//                ////PermanentDelta has EndTime
//                //Assert.IsFalse(result.IsOk);
//                //Console.WriteLine(result.ErrorMessage);


//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.TempDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                //TempDelta does not have EndTime
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);

//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.TempDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1990, 1, 1), new DateTime(1980, 1, 1)),
//                    Data = data
//                });
//                //EndPosition should be greater or equal than BeginPosition
//                Assert.IsFalse(result.IsOk);
//                Console.WriteLine(result.ErrorMessage);

//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1990, 1, 1)),
//                    Data = data
//                });
//                //all is ok
//                Assert.IsTrue(result.IsOk);


//                data.Name = "ASAD2";
//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1995, 1, 1)),
//                    Data = data
//                });
//                //all is ok
//                Assert.IsTrue(result.IsOk);


//                //result = temporalityService.CommitNewEvent(new AimEvent
//                //{
//                //    Interpretation = Interpretation.PermanentDelta,
//                //    TimeSlice = new TimeSlice(new DateTime(1995, 1, 1)),
//                //    Data = data
//                //});
//                ////same start
//                //Assert.IsFalse(result.IsOk);
//                //Console.WriteLine(result.ErrorMessage);

//                var mask = new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport, Guid = TestGuid };

//                var res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1991, 1, 1))[0];
//                var item1 = res.Data as AirportHeliport;
//                Debug.Assert(item1 != null, "item1 != null");
//                var name1 = item1.Name;
//                Assert.IsTrue(name1=="ASAD");

//                res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1996, 1, 1))[0];
//                item1 = res.Data as AirportHeliport;
//                Debug.Assert(item1 != null, "item1 != null");
//                name1 = item1.Name;
//                Assert.IsTrue(name1 == "ASAD2");


//                //res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1796, 1, 1))[0];
//                //Assert.IsTrue(res==null);

//                //var list = temporalityService.GetSnapshotByFeatureName(mask, new DateTime(1996, 1, 1));
//                //var item = (list[0].Data as AirportHeliport);
//                //Debug.Assert(item != null, "item != null");
//                //var name = item.Name;
//                //Assert.IsTrue(name == "ASAD2");


//                var list2 = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1796, 1, 1));
//                Assert.IsTrue(0==list2.Count);
//            }
//        }

//        [TestMethod]
//        public void CancelEvent()
//        {
//            //should be ok
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                data.Name = "ASAD2";
//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 2, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                var mask = new FeatureId
//                               {
//                    FeatureTypeId = (int)FeatureType.AirportHeliport,
//                    Guid = TestGuid
//                };

//                var result2 = temporalityService.CancelSequence(new TimeSliceId(mask) { Version = new TimeSliceVersion(2, 0) });
//                Assert.IsTrue(result2.IsOk);

//                result2 = temporalityService.CancelSequence(new TimeSliceId(mask) { Version = new TimeSliceVersion(2, 0) });
//                Assert.IsFalse(result2.IsOk);
//                //already cancelled
//                Console.WriteLine(result2.ErrorMessage);

//                var res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1999, 1, 1))[0];
//                var item1 = res.Data as AirportHeliport;
//                Debug.Assert(item1 != null, "item1 != null");
//                var name1 = item1.Name;
//                Assert.IsTrue(name1 == "ASAD");

//                var list = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);
//                var item = (list[0].Data as AirportHeliport);
//                Debug.Assert(item != null, "item != null");
//                var name = item.Name;
//                Assert.IsTrue(name == "ASAD");
//            }



//        }

//        [TestMethod]
//        public void CancelFeature()
//        {
//            //should be ok
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                data.Name = "ASAD2";
//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 2, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                var mask = new FeatureId
//                               {
//                    FeatureTypeId = (int)FeatureType.AirportHeliport,
//                    Guid = TestGuid
//                };

//                var result2 = temporalityService.CancelSequence(new TimeSliceId(mask) { Version = new TimeSliceVersion(1, 0) });
//                Assert.IsFalse(result2.IsOk);
//                //2.0 still exist 
//                Console.WriteLine(result2.ErrorMessage);

//                result2 = temporalityService.CancelSequence(new TimeSliceId(mask) { Version = new TimeSliceVersion(2, 0) });
//                Assert.IsTrue(result2.IsOk);

//                result2 = temporalityService.CancelSequence(new TimeSliceId(mask) { Version = new TimeSliceVersion(1, 0) });
//                Assert.IsTrue(result2.IsOk);

//                var list = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);
//                Assert.IsTrue(list.Count==0);

//            }



//        }

//        [TestMethod]
//        public void Correction()
//        {
//            //should be ok
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                data.Name = "ASAD2";
//                result = temporalityService.CommitCorrection(new AimEvent
//                {
//                    Version = new TimeSliceVersion(1,1),
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 2, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                var mask = new FeatureId
//                               {
//                    FeatureTypeId = (int)FeatureType.AirportHeliport,
//                    Guid = TestGuid
//                };


//                var res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1999, 1, 1))[0];
//                Debug.Assert(res != null, "res != null");
//                var item1 = res.Data as AirportHeliport;
//                Debug.Assert(item1 != null, "item1 != null");
//                var name1 = item1.Name;
//                Assert.IsTrue(name1 == "ASAD2");

//                var list = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, DateTime.Now);
//                var item = (list[0].Data as AirportHeliport);
//                Debug.Assert(item != null, "item != null");
//                var name = item.Name;
//                Assert.IsTrue(name == "ASAD2");
//            }



//        }

//         [TestMethod]
//        public void GetEvent()
//        {
//            //should be ok
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1991, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                data.Name = "ASAD2";
//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1992, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);
              

//                var mask = new FeatureId
//                               {
//                    FeatureTypeId = (int)FeatureType.AirportHeliport,
//                    Guid = TestGuid
//                };

//                var result2=temporalityService.GetEvent(mask, new TimeSliceVersion(2, 0));
//                Assert.IsTrue(result2!=null);


//                result2 = temporalityService.GetEvent(mask, new TimeSliceVersion(3, 0));
//                Assert.IsTrue(result2 == null);
//            }



//        }
        
//         [TestMethod]
//         public void GetEventMeta()
//        {
//            //should be ok
//            using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//            {
//                temporalityService.Truncate();

//                var data = new AirportHeliport
//                               {
//                    Identifier = TestGuid,
//                    Name = "ASAD"
//                };

//                var result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1990, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);

//                data.Name = "ASAD2";
//                result = temporalityService.CommitNewEvent(new AimEvent
//                {
//                    Interpretation = Interpretation.PermanentDelta,
//                    TimeSlice = new TimeSlice(new DateTime(1995, 1, 1)),
//                    Data = data
//                });
//                Assert.IsTrue(result.IsOk);
              

//                var mask = new FeatureId
//                               {
//                    FeatureTypeId = (int)FeatureType.AirportHeliport,
//                    Guid = TestGuid
//                };

//                var result2 = temporalityService.GetActualEventMeta(mask, null,null);
//                Assert.IsTrue(result2!=null);
//                Assert.IsTrue(result2.Count==2);

//                result2 = temporalityService.GetActualEventMeta(mask, new TimeSlice(new DateTime(1992,1,1)), null);
//                Assert.IsTrue(result2 != null);
//                Assert.IsTrue(result2.Count == 1);

//                result2 = temporalityService.GetActualEventMeta(mask, new TimeSlice(new DateTime(1982, 1, 1),new DateTime(1991,1,1)), null);
//                Assert.IsTrue(result2 != null);
//                Assert.IsTrue(result2.Count == 1);


//                result2 = temporalityService.GetActualEventMeta(mask, new TimeSlice(new DateTime(1982, 1, 1), new DateTime(1984, 1, 1)), null);
//                Assert.IsTrue(result2 != null);
//                Assert.IsTrue(result2.Count == 0);

//            }



//        }


//         [TestMethod]
//         public void CommitEventSequence()
//         {
//             using (var temporalityService = AimServiceFactory.CreateLocal("Test1"))
//             {
//                 temporalityService.Truncate();

//                 var data = new AirportHeliport
//                 {
//                     Identifier = TestGuid,
//                     Name = "ASAD"
//                 };

//                 var result = temporalityService.CommitNewEvent(new AimEvent
//                 {
//                     Interpretation = Interpretation.PermanentDelta,
//                     TimeSlice = new TimeSlice(new DateTime(1995, 1, 1)),
//                     Data = data
//                 });
//                 Assert.IsTrue(result.IsOk);


//                 data.Name = "ASAD2";
//                 result = temporalityService.CommitNewEvent(new AimEvent
//                 {
//                     Interpretation = Interpretation.PermanentDelta,
//                     TimeSlice = new TimeSlice(new DateTime(1990, 1, 1)),
//                     Data = data
//                 });
//                 Assert.IsTrue(result.IsOk);

                


//                 var mask = new FeatureId { FeatureTypeId = (int)FeatureType.AirportHeliport, Guid = TestGuid };

//                 var res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1996, 1, 1))[0];
//                 var item1 = res.Data as AirportHeliport;
//                 Debug.Assert(item1 != null, "item1 != null");
//                 var name1 = item1.Name;
//                 Assert.IsTrue(name1 == "ASAD");

//                 res = temporalityService.GetActualDataByDate(Interpretation.BaseLine, mask, null, new DateTime(1994, 1, 1))[0];
//                 item1 = res.Data as AirportHeliport;
//                 Debug.Assert(item1 != null, "item1 != null");
//                 name1 = item1.Name;
//                 Assert.IsTrue(name1 == "ASAD2");


               
//             }
//         }
//    }
//}
