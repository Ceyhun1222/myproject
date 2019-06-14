#region

using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.ServiceModel;
using Aran.Aim;
using Aran.Aim.Features;
using Aran.Temporality.Aim.Storage;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.Remote.Protocol;
using Aran.Temporality.Internal.Remote.Util;
using Aran.Temporality.Util;
using log4net;
using log4net.Core;

#endregion

namespace Aran.Temporality.Internal.Remote.Service
{
    internal class TemporalityService : ITemporalityService
    {
        public static readonly ILog Log = LogManager.GetLogger(typeof(TemporalityService));

        #region Util

        private object ProcessCommunicationRequest(CommunicationRequest request)
        {

            IIdentity identity = AccessUtil.CurrentIdentity();
            MethodInfo methodInfo = typeof(AimStorage).GetMethod(request.MethodName);

            Guid? workpackageId = null;
            int featureTypeId = -1;

            foreach(var param in request.Params)
            {
                if (param is IFeatureId)
                {
                    var featureId = param as IFeatureId;
                    workpackageId = featureId.WorkPackage;
                    featureTypeId = featureId.FeatureTypeId;
                    break;
                }
            }

            var operations = (SecureOperation[]) methodInfo.GetCustomAttributes(typeof(SecureOperation), true);
            bool accessGranted = operations.All(operation =>
                AccessUtil.AccessGranted(identity, request.Path, workpackageId, featureTypeId, operation.Operation));

            string operationString = operations.Aggregate(string.Empty, (current, operation) => current + (operation.Operation.ToString() + " "));

            Console.WriteLine(@"SERVER MESSAGE: Storage: " + request.Path + @", User: " +
              identity.Name + @", Methods: " + request.MethodName + @", Operations: " + 
              operationString + @", Access: " + accessGranted);

            //Log.Info("Storage: " + request.Path + ", User: " + identity.Name +
            //    ", Methods: " + request.MethodName + ", Operations: " + operationString + 
            //    ", Access: " + accessGranted);

            
            Log.Logger.Log(new LoggingEvent(new LoggingEventData
            {
            Level = Level.Info,
            LocationInfo = new LocationInfo(identity.Name, request.MethodName + "[" + operationString + "]", request.Path + (workpackageId == null ? "" : "\\" + workpackageId.ToString()) + (featureTypeId == -1 ? "" : "\\" + (FeatureType)featureTypeId), (accessGranted ? "Access granted" : "Access denied")),
            LoggerName                  = "TemporalityService",
            TimeStamp = DateTime.Now
            }));


            if (accessGranted)
            {
                IStorage<Feature> storage = AimStorageFactory.GetLocalStorage(request.Path);
                object result = methodInfo.Invoke(storage, request.Params);
                return result;
            }

            throw new Exception("Access denied");
        }

        #endregion

        #region Implementation of ITemporalityService

        public byte[] ProcessBinaryMessage(byte[] binary)
        {
            var request = FormatterUtil.ObjectFromBytes<CommunicationRequest>(binary);
            var response = new CommunicationResponse();

            try
            {
                response.ResultObject = ProcessCommunicationRequest(request);
            }
            catch (Exception ex)
            {
                response.OperationResult.ReportError(ex.Message);
            }

            return FormatterUtil.ObjectToBytes(response);
        }

        #endregion
    }
}