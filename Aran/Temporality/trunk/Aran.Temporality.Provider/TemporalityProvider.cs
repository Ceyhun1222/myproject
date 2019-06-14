using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Aran.Aim;
using Aran.Aim.Data;
using Aran.Aim.Data.Filters;
using Aran.Aim.DataTypes;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Converters;
using Aran.Geometries;
using Aran.Temporality.Common.Abstract.Event;
using Aran.Temporality.Common.Abstract.State;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Enum;
using Aran.Temporality.Common.Id;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.CommonUtil.Context;
using TimeSlice = Aran.Temporality.Common.MetaData.TimeSlice;
using Aran.Aim.Utilities;
using Aran.Aim.Objects;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Util;
using Aran.Temporality.CommonUtil.Util;
using User = Aran.Aim.Data.User;

namespace Aran.Temporality.Provider
{


    public class TemporalityProvider : DbProvider
    {
        private Dictionary<FeatureType, IList<AbstractState<AimFeature>>> cache = new Dictionary<FeatureType, IList<AbstractState<AimFeature>>>();
        private DateTime CachedDate = new DateTime();

        // most recently added by Kaveh request 
        private FeatureReport UnzipReport(FeatureReportZipped zipped)
        {
            return new FeatureReport
            {
                Identifier = Guid.Parse(zipped.FeatureGuid),
                HtmlZipped = zipped.ReportData,
                ReportType = (FeatureReportType)Enum.Parse(typeof(FeatureReportType), zipped.ReportType, true),
                DateTime = zipped.DateTime
            };
        }

        private FeatureReportZipped ZipReport(FeatureReport report)
        {
            return new FeatureReportZipped
            {
                FeatureGuid = report.Identifier.ToString(),
                ReportData = report.HtmlZipped,
                ReportType = report.ReportType.ToString()
            };
        }

        public override List<FeatureReport> GetFeatureReport(Guid identifier)
        {
            var zippedList = _noAixmDataService.GetFeatureReportsByIdentifier(identifier.ToString());
            return zippedList.Select(UnzipReport).ToList();
        }

        public override void SetFeatureReport(FeatureReport report)
        {
            _noAixmDataService.SaveFeatureReport(ZipReport(report));
        }



        private FeatureScreenshot ConvertScreenshot(Screenshot screenshot)
        {
            return new FeatureScreenshot
            {
                FeatureGuid = screenshot.Identifier.ToString(),
                ScreenshotData = screenshot.Images,
                DateTime = screenshot.DateTime
            };
        }


        private Screenshot ConvertScreenshot(FeatureScreenshot screenshot)
        {
            return new Screenshot
            {
                Identifier = Guid.Parse(screenshot.FeatureGuid),
                Images = screenshot.ScreenshotData,
                DateTime = screenshot.DateTime
            };
        }

        public override List<Screenshot> GetFeatureScreenshot(Guid identifier)
        {
            var list = _noAixmDataService.GetFeatureScreenshotsByIdentifier(identifier.ToString());
            return list.Select(ConvertScreenshot).ToList();
        }

        public override void SetFeatureScreenshot(Screenshot screenshot)
        {
            _noAixmDataService.SaveFeatureScreenshot(ConvertScreenshot(screenshot));
        }
        //---end of Kaveh request


        public override GettingResult GetVersionsOf(FeatureType featType, TimeSliceInterpretationType interpretation, TimePeriod submissionTime, Guid identifier = default(Guid), bool loadComplexProps = false, TimeSliceFilter timeSlicefilter = null, List<string> propList = null, Filter filter = null)
        {
            throw new NotImplementedException();
        }

        private void ClearCache()
        {
            cache.Clear();
        }
        //private StreamWriter logFile;

        //private void Log(string s)
        //{
        //    logFile.WriteLine(s);
        //    logFile.Flush();
        //}

        public TemporalityProvider()
        {
            CurrentUser = new User()
            {
                Name = "administrator",
                Privilege = Privilige.prAdmin
            };
            CurrentUser.FeatureTypes.AddRange(typeof(FeatureType).GetEnumValues().Cast<int>());


        }
        private ITemporalityService<AimFeature> _temporalityService;
        private INoAixmDataService _noAixmDataService;

        #region Converter

        private AbstractEvent<AimFeature> GetEventFromData(Feature feature)
        {
            var e = new AimEvent
            {
                TimeSlice = new TimeSlice
                {
                    BeginPosition = feature.TimeSlice.ValidTime.BeginPosition,
                    EndPosition = feature.TimeSlice.ValidTime.EndPosition,
                },
                Version = new TimeSliceVersion(feature.TimeSlice.SequenceNumber, feature.TimeSlice.CorrectionNumber),
                Interpretation = feature.TimeSlice.Interpretation != TimeSliceInterpretationType.TEMPDELTA? Interpretation.PermanentDelta : Interpretation.TempDelta,

                Data = feature,
            };


            if (feature.TimeSlice == null)
            {
                //MessageBox.Show(@"TimeSlice is null",@"Warning");
                //TODO: show message about null TimeSlice
            }
            else if (feature.TimeSlice.FeatureLifetime == null)
            {
                //MessageBox.Show(@"LifeTime is null", @"Warning");
                //TODO: show message about null TimeSlice
            }
            else
            {
                e.LifeTimeBegin = feature.TimeSlice.FeatureLifetime.BeginPosition;
                if (feature.TimeSlice.FeatureLifetime.EndPosition != null)
                {
                    e.LifeTimeEnd = feature.TimeSlice.FeatureLifetime.EndPosition;
                }
            }
            return e;
        }


        private Feature GetDataFromState(AbstractState<AimFeature> myEvent)
        {
            //TODO:implement
#warning fix
            var result = myEvent.Data;
            return result.Feature;
        }

        #endregion

        #region Implementation of DbProvider

        public override DbProviderType GetProviderType(ref string otherName)
        {
            return DbProviderType.TDB;
        }

        public override List<string> GetConnectionStrings()
        {
            return ConnectionProvider.GetConnectionStrings();
        }

        public override void Open(string connectionString)
        {
            State = ConnectionState.Broken;

            ConnectionProvider.Open(connectionString);
            _temporalityService = CurrentDataContext.CurrentService;
            _noAixmDataService = CurrentDataContext.CurrentNoAixmDataService;

            State = ConnectionState.Open;

            var filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/RISK/ARAN/_log.txt";
            //logFile = new StreamWriter(CurrentDataContext.ProviderLogFile ?? "_log.txt", false);
            //logFile = new StreamWriter(filePath, false);
            CurrentUser.Name = CurrentDataContext.CurrentUser?.Name;

        }

        public override bool Login(string userName, string md5Password)
        {
            var user = CurrentDataContext.UserDto;

            if (user != null)
            {
                CurrentUser = new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Surname = user.Surname,
                    Address = user.Address,
                    City = user.City,
                    Country = user.Country,
                    DataOriginatorCode = user.DataOriginatorCode,
                    Department = user.Department,
                    Email = user.Email,
                    FaxNumber = user.FaxNumber,
                    Organization = user.Organization,
                    Position = user.Position,
                    Zip = user.Zip,
                    Privilege = Privilige.prAdmin
                };
            }
            else
            {
                CurrentUser = new User {UserName = userName};
            }

            CurrentUser.FeatureTypes.AddRange(Enum.GetValues(typeof(FeatureType)).Cast<int>());
            return true;
        }

        public override List<User> GetOriginators()
        {
            return CommonDataProvider.GetUsers()
                .Select(user => new User
                {
                    Id = user.Id,
                    Name = user.Name,
                    UserName = user.UserName,
                    Surname = user.Surname,
                    Address = user.Address,
                    City = user.City,
                    Country = user.Country,
                    DataOriginatorCode = user.DataOriginatorCode,
                    Department = user.Department,
                    Email = user.Email,
                    FaxNumber = user.FaxNumber,
                    Organization = user.Organization,
                    Position = user.Position,
                    Zip = user.Zip,
                    Privilege = Privilige.prAdmin
                })
                .ToList();
        }

        public override void Close()
        {
            //logFile.Close();
            ConnectionProvider.Close();
            State = ConnectionState.Closed;
        }

        public override bool IsExists(Guid guid)
        {
            return false;
        }

        public override bool IsExists(Guid guid, FeatureType type)
        {
            return _temporalityService.GetActualDataByDate(new FeatureId { Guid = guid, FeatureTypeId = (int)type}, false, DefaultEffectiveDate).Count > 0;
        }

        public override InsertingResult Insert(Feature feature, bool insertAnyway, bool asCorrection)
        {
            var newTransactionId = BeginTransaction();

            var insertResult = Insert(feature, newTransactionId, insertAnyway, asCorrection);

            if (!insertResult.IsSucceed)
            {
                Rollback(newTransactionId);
                return insertResult;
            }

            return Commit(newTransactionId);
        }

        public TimeSliceFilter TimeSliceFilter { get; set; }

        public override List<Feature> GetAllFeatuers(FeatureType featType, Guid identifier = default(Guid))
        {
            return (List<Feature>)GetVersionsOf(featType, TimeSliceInterpretationType.BASELINE, identifier).List;
        }

        private DateTime _defaultEffectiveDate = DateTime.Now;
        public override DateTime DefaultEffectiveDate
        {
            get => _defaultEffectiveDate;
            set
            {
                _defaultEffectiveDate = value;
                if (DefaultEffectiveDate != CachedDate)
                {
                    ClearCache();
                    CachedDate = DefaultEffectiveDate;
                }
            }
        }

        #endregion

        #region Important

        public override int BeginTransaction()
        {
            return 0;
        }

        public override InsertingResult Update(Feature feature, int transactionId)
        {
            // Temporary solution to integrate ADM & TOSS
            feature.TimeSlice.CorrectionNumber = 1;
            return Insert(feature, transactionId, true, false);
        }

        public override InsertingResult Insert(Feature feature, int transactionId, bool insertAnyway, bool asCorrection)
        {
            var result = new InsertingResult(true);
            try
            {
                feature.TimeSlice.CorrectionNumber = 0;
                var myEvent = GetEventFromData(feature);
                myEvent.WorkPackage = transactionId;

                if (asCorrection)
                {
                    _temporalityService.CommitNewEvent(myEvent);
                }
                else
                {
                    _temporalityService.CommitNewEvent(myEvent);
                }
            }
            catch (Exception exception)
            {
                result.Message = exception.Message;
                result.IsSucceed = false;
            }
            return result;
        }

        public override InsertingResult Commit(int transactionId)
        {
            return new InsertingResult(true);
        }

        public override InsertingResult Rollback(int transactionId)
        {
            return new InsertingResult(true);
        }

        ///// <summary>
        ///// Gets version(s) of feature for given identifer
        ///// </summary>
        ///// <typeparam name="T">Wich feature type you want to get</typeparam>
        ///// <param name="identifier">Identifier of given feature(T)</param>
        ///// <param name="loadComplexProp">Determines to load complex types or not</param>
        ///// <param name="propInfoList">Which properties you want to be fulled.If there is object property to get then loadComplexTypes have to be true</param>
        ///// <param name="timeSliceFilter">Condition that define your query</param>
        ///// <returns>Returns list of T feature for given identifier and condition</returns>
        public override GettingResult GetVersionsOf(FeatureType featType,
                                    TimeSliceInterpretationType interpretation,
                                    Guid identifier = default(Guid),
                                    bool loadComplexProps = false,
                                    TimeSliceFilter timeSlicefilter = null,
                                    List<string> propList = null,
                                    Filter filter = null)
        {
            //TODO: work with interpretation
            //TODO: work with propList

            System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
            st.Start();

            var myfilter = TimeSliceFilter;
            if (timeSlicefilter != null) myfilter = timeSlicefilter;

            if (myfilter == null)
            {
                myfilter = new TimeSliceFilter(DefaultEffectiveDate);
                //return new GettingResult(false, "TimeSliceFilter is null");
            }


            if (filter != null && filter.Operation == null)
            {
                return new GettingResult(false, "Wrong filter was specified");
            }

            var mask = new FeatureId
            {
                FeatureTypeId = (int)featType,
            };

            if (identifier != default(Guid))
            {
                mask.Guid = identifier;
            }

            var result = new GettingResult(true) { List = new List<Feature>() };


            if (false && filter != null && filter.Operation.Choice == OperationChoiceType.Spatial)
            {
                Geometry geo = null;
                double distance = 0;

                var inExtend = filter.Operation.SpatialOps as InExtend;
                if (inExtend != null)
                {
                    var extendPolygon = new Polygon
                    {
                        ExteriorRing = new Ring
                        {
                            new Point(inExtend.MinX, inExtend.MinY),
                            new Point(inExtend.MaxX, inExtend.MinY),
                            new Point(inExtend.MaxX, inExtend.MaxY),
                            new Point(inExtend.MinX, inExtend.MaxY),
                            new Point(inExtend.MinX, inExtend.MinY)
                        }
                    };

                    geo = extendPolygon;
                }
                var within = filter.Operation.SpatialOps as Within;
                if (within != null)
                {
                    geo = within.Geometry;
                }

                var dWithin = filter.Operation.SpatialOps as DWithin;
                if (dWithin != null)
                {

                    distance = ConverterToSI.Convert(dWithin.Distance, double.NaN);
                }


                var resultList = _temporalityService.GetActualDataByGeo(mask, false, myfilter.EffectiveDate, geo, distance);
                foreach (var state in resultList)
                {
                    result.List.Add(GetDataFromState(state));
                }

                st.Stop();

                var elapsedGeo = (long)st.Elapsed.TotalMilliseconds;
                totalElapsed += elapsedGeo;


                //Log(DateTime.Now.ToString("hh:mm:ss") + " FeatureType=" + featType +
                //    (mask.Guid != null ? " Id=" + identifier : "") +
                //    (filter != null ? " Filter=" + FilterToString(filter) : "") +
                //    " ElapsedGeo=" + elapsedGeo + "ms Total=" + totalElapsed + "ms");

                result.List = FilterDecomissingFeature(result.List, myfilter.EffectiveDate);
                return result;
            }
            else
            {
                IList<AbstractState<AimFeature>> list;
                if (mask.Guid == null)
                {

                    if (base.UseCache)
                    {
                        if (!cache.TryGetValue(featType, out list))
                        {
                            list = _temporalityService.GetActualDataByDate(mask, false, myfilter.EffectiveDate);
                            cache[featType] = list;
                        }
                    }
                    else
                    {
                        list = _temporalityService.GetActualDataByDate(mask, false, myfilter.EffectiveDate);
                    }

                }
                else
                {
                    list = _temporalityService.GetActualDataByDate(mask, false, myfilter.EffectiveDate);
                }

                if (list != null)
                {
                    foreach (var state in list.Where(state => filter == null || state.Data.Feature.IsFilterOk(filter)))
                    {
                        result.List.Add(GetDataFromState(state));
                    }
                }
            }


            result.List = FilterDecomissingFeature(result.List, myfilter.EffectiveDate);


            st.Stop();
            var elapsed = (long)st.Elapsed.TotalMilliseconds;
            totalElapsed += elapsed;


            //Log(DateTime.Now.ToString("hh:mm:ss") + " FeatureType=" + featType +
            //    (mask.Guid != null ? " Id=" + identifier : "") +
            //    (filter != null ? " Filter=" + FilterToString(filter) : "") +
            //    " Elapsed=" + elapsed + "ms Total=" + totalElapsed + "ms");

            return result;
        }

        private List<Feature> FilterDecomissingFeature(IList list, DateTime effDate)
        {
            var featList = new List<Feature>();
            var dt1 = new DateTime(effDate.Year, effDate.Month, effDate.Day, 0, 0, 0);

            foreach (Feature feat in list)
            {

                if (feat.TimeSlice == null || feat.TimeSlice.FeatureLifetime == null || feat.TimeSlice.FeatureLifetime.EndPosition == null)
                {
                    // This code is present due to bug in toss db
                    if (feat.TimeSlice?.ValidTime?.EndPosition != null && feat.TimeSlice.ValidTime.EndPosition <= feat.TimeSlice.ValidTime.BeginPosition)
                        continue;

                    featList.Add(feat);
                }
                else
                {
                    var dt2 = feat.TimeSlice.FeatureLifetime.EndPosition.Value;
                    dt2 = new DateTime(dt2.Year, dt2.Month, dt2.Day, 0, 0, 0);

                    if (dt1 < dt2)
                        featList.Add(feat);
                }
            }
            return featList;
        }

        private static string FilterToString(Filter filter)
        {
            return filter.Operation.Choice.ToString();
        }

        long totalElapsed = 0;


        static object GetPropertyValue(object data, string property)
        {
            var propInfos = AimMetadataUtility.GetInnerProps((int)(data as Feature).FeatureType, property);
            var propInfo = propInfos.LastOrDefault();

            if (propInfo == null)
                return null;

            var propValList = AimMetadataUtility.GetInnerPropertyValue((data as IAimObject), propInfos, false);
            var propVal = propValList.FirstOrDefault();

            if (propVal is IEditAimField)
                return (propVal as IEditAimField).FieldValue;
            if (propVal is Aran.Aim.DataTypes.FeatureRef)
                return (propVal as Aran.Aim.DataTypes.FeatureRef).Identifier;




            return propVal;


            var i = property.IndexOf("/");
            if (i == -1) i = property.IndexOf(".");
            var currentProperty = i > -1 ? property.Substring(0, i) : property;
            var remainingProperty = i > -1 ? property.Substring(i + 1, property.Length - i - 1) : null;

            var pi = data.GetType().GetProperties().Where(t => t.Name.ToLower() == currentProperty.ToLower()).FirstOrDefault();
            if (pi == null) throw new Exception("wrong property name");
            object currentValue = null;
            try
            {
                currentValue = pi.GetValue(data, null);
            }
            catch
            {
            }

            if (currentValue is IList)
            {
                var list = currentValue as IList;
                if (list.Count > 0)
                {
                    currentValue = list[0];
                }
                else
                {
                    currentValue = null;
                }
            }

            if (currentValue == null) return null;

            if (remainingProperty == null) return currentValue;


            return GetPropertyValue(currentValue, remainingProperty);
        }

        #endregion

        public override void CallSpecialMethod(string methodName, object arg)
        {
            if (methodName == "SetApplicationEnvironment")
            {
                Aran.Temporality.CommonUtil.Context.CurrentDataContext.Application = "Environment";
            }
            else if (methodName == "SelectSlot")
            {
                var result = Aran.Temporality.CommonUtil.Util.SlotSelector.ShowDialog();
                (arg as dynamic).Result = result;
            }
        }


    }
}
