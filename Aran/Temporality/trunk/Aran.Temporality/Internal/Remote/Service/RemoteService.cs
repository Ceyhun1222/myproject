#region

using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Aran.Aim;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Entity;
using Aran.Temporality.Common.Entity.Enum;
using Aran.Temporality.Common.Exceptions;
using Aran.Temporality.Common.Extensions;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.Common.Logging;
using Aran.Temporality.Common.Util;
using Aran.Temporality.Internal.Attribute;
using Aran.Temporality.Internal.Implementation.Storage;
using Aran.Temporality.Internal.Remote.Protocol;
using Aran.Temporality.Internal.Remote.Util;
using Aran.Temporality.Internal.Service;
using NHibernate.Linq;

//using log4net;

#endregion

namespace Aran.Temporality.Internal.Remote.Service
{
    internal class RemoteService : IRemoteService
    {
        //public static readonly ILog Log = LogManager.GetLogger(typeof(RemoteService));

        #region Util

        private object ProcessCommunicationRequest(CommunicationRequest request)
        {

            var identity = StorageService.CurrentIdentity();

            if (!identity.IsAuthenticated) throw new Exception("Access denied");


            OperationContext context = OperationContext.Current;
            MessageProperties prop = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string ip = endpoint == null ? "unknown" : endpoint.Address;


            var userName = identity.Name;
            var i = userName.IndexOf("\\", StringComparison.Ordinal);
            var userId = userName.Substring(0, i);

            var application = userName.Substring(i + 1);
            var j = application.IndexOf(":", StringComparison.Ordinal);
            var applicationVersion = "1.0.0.0";
            if (j > -1)
            {
                applicationVersion = application.Substring(j + 1);
                application = application.Substring(0, j);
            }



            var user = StorageService.GetUserById(Convert.ToInt32(userId));
            WcfOperationContext.Current.Items["user"] = user;

            if (request.MethodName == "Login")
            {
                //save UserModuleVersion
                var userModuleVersion = new UserModuleVersion
                {
                    ActualVersion = applicationVersion,
                    Module = application,
                    User = user
                };
                StorageService.UpdateUserModuleVersion(userModuleVersion);
            }

            NoAixmDataService.PreprocessCall(request);

            var methodInfo = typeof(AimTemporalityServiceProxy).GetMethod(request.MethodName);

            var workpackageId = -1;
            var featureTypeId = -1;
            Guid? featureGuid = null;

            var slotId = -1;
            if (user.ActivePrivateSlot != null)
            {
                slotId = user.ActivePrivateSlot.Id;
            }

            //set wp
            foreach (var featureId in request.Params.OfType<IFeatureId>())
            {
                if (slotId > -1)
                {
                    var wp = ((dynamic)featureId).WorkPackage;
                    if (wp == 0)
                    {
                        ((dynamic)featureId).WorkPackage = slotId;
                    }
                    if(wp == -1)
                    {
                        ((dynamic)featureId).WorkPackage = 0;
                    }
                }

                workpackageId = featureId.WorkPackage;
                featureTypeId = featureId.FeatureTypeId;
                featureGuid = featureId.Guid;
                break;
            }


            var operations = (SecureOperation[])methodInfo.GetCustomAttributes(typeof(SecureOperation), true);
            bool accessGranted = operations.All(operation =>
                StorageService.AccessGranted(workpackageId, featureTypeId, operation.Operation));



            if ((user.RoleFlag & (int)UserRole.Observer) != 0)
            {
                if (operations.Length == 1 && operations[0].Operation == InternalOperation.ReadData)
                {
                    accessGranted = true;
                }
            }

            if ((user.RoleFlag & (int)UserRole.Tester) != 0)
            {
                accessGranted = true;
            }

            
            string operationString = operations.Aggregate(string.Empty, (current, operation) => current + operation.Operation.ToString() + " ");


            var logInfos = (LogOperation[])methodInfo.GetCustomAttributes(typeof(LogOperation), true);




            if (!string.IsNullOrWhiteSpace(operationString) || logInfos.Length > 0)
            {
                string logParameterString = null;
                if (logInfos.Length > 0)
                {
                    var methodParameters = methodInfo.GetParameters();
                    var logActionString = string.IsNullOrWhiteSpace(logInfos[0].Action)
                        ? string.Empty
                        : logInfos[0].Action;
                    logParameterString = logInfos[0].Arguments == null
                        ? string.Empty
                        : logInfos[0].Arguments.Aggregate(string.Empty, (current, logArgument) =>
                            current + (logArgument >= 0 && logArgument < request.Params.Length
                                ? ", " + methodParameters[logArgument].Name + " : " +
                                  request.Params[logArgument].ToString()
                                : string.Empty + " "));

                    operationString = string.IsNullOrWhiteSpace(operationString)
                        ? logActionString
                        : operationString + " " + logActionString;
                }


                var logEntry = new LogEntry
                {
                    Storage = request.Storage,
                    UserName = user.Name,
                    UserId = user.Id,
                    Date = DateTime.Now,
                    AccessGranted = accessGranted,
                    Action = operationString,
                    Ip = ip,
                    Application = application,
                    Parameters = request.MethodName +
                                 (workpackageId == -1
                                     ? string.Empty
                                     : ", Space : " +
                                       (user.ActivePrivateSlot != null && user.ActivePrivateSlot.Id == workpackageId
                                           ? user.ActivePrivateSlot.Name
                                           : "Other [id=" + workpackageId + "]") + " ") +
                                 (featureTypeId == -1
                                     ? string.Empty
                                     : ", FeatureType : " + (FeatureType)featureTypeId + " ") +
                                 (featureGuid == null ? string.Empty : ", FeatureId : " + featureGuid + " ") +
                                 (string.IsNullOrWhiteSpace(logParameterString) ? string.Empty : logParameterString)
                };
                StorageService.Log(logEntry);
                Console.WriteLine(@"Storage: " + request.Storage +
                                  @", User: " + user.Name +
                                  @", Methods: " + request.MethodName +
                                  @", Feature: " +
                                  (featureTypeId == -1 ? "N/A" : ((FeatureType)featureTypeId).ToString()) +
                                  @", Operations: " + operationString +
                                  @", Access: " + accessGranted);
            }



            var logString =
                $@"Storage: {request.Storage}, UserId: {user.Id}, User: {user.Name}, Ip: {ip}, Methods: {
                    request.MethodName}, Operations: {
                    operationString}, Access: {accessGranted}";
            LogManager.GetLogger(typeof(RemoteService)).Info(logString);
            LogManager.GetLogger(typeof(RemoteService)).Debug(request.Dump);

            if (accessGranted)
            {
                if (request.Storage == null)
                {
                    StorageService.Init();
                    var temporalityService = StorageService.GetNonDataService();
                    var result = methodInfo.Invoke(temporalityService, request.Params);
                    return result;
                }
                else
                {
                    if (slotId > -1 && operations.Any(t => t.Operation == InternalOperation.WriteData))
                    {
                        //update slot status

                        if (user.ActivePrivateSlot == null)
                        {
                            LogManager.GetLogger(typeof(RemoteService)).Warn($"No active slot selected for write operation. {logString}");
                            throw new OperationException("No active slot selected for write operation.");
                        }

                        user.ActivePrivateSlot.Status = SlotStatus.Opened;
                        StorageService.UpdatePrivateSlot(user.ActivePrivateSlot);
                    }

                    var temporalityService = AimServiceFactory.OpenLocal(request.Storage);
                    var result = methodInfo.Invoke(temporalityService, request.Params);
                    return result;
                }
            }

            LogManager.GetLogger(typeof(RemoteService)).Warn($"Access denied: {logString}.");
            throw new AccessDeniedException("Access denied");
        }

        #endregion

        #region Implementation of IRemoteService

        public byte[] ProcessBinaryMessage(byte[] binary)
        {
            var request = FormatterUtil.ObjectFromBytes<CommunicationRequest>(binary);
            var response = new CommunicationResponse();

            try
            {
                response.ResultObject = ProcessCommunicationRequest(request);
            }
            catch (AccessDeniedException ex)
            {
                response.OperationResult.ReportError(ex.Message);
            }
            catch (OperationException ex)
            {
                response.OperationResult.ReportError(ex.Message);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof (OperationException))
                    response.OperationResult.ReportError(ex.InnerException.Message);
                else
                {
                    LogManager.GetLogger(typeof (RemoteService)).Error(ex, "Server Exception");
                    response.OperationResult.ReportError(ex.Message);
                }
            }

            return FormatterUtil.ObjectToBytes(response);
        }

        #endregion
    }
}