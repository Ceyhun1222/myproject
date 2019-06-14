using AerodromeServices.DataContract;
using AerodromeServices.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace AerodromeServices.Helpers
{
    public class CustomParameterInspector : IParameterInspector
    {
        private int _index = 0;

        public CustomParameterInspector()
            : this(0)
        { }

        public CustomParameterInspector(int index)
        {
            _index = index;
        }

        #region IParameterInspector Member Implementation

        public void AfterCall(string operationName, object[] outputs, object returnValue, object correlationState)
        {
            DoLog(operationName, outputs,false);
        }

        public object BeforeCall(string operationName, object[] inputs)
        {
            DoLog(operationName, inputs);
            return null;
        }

        private void DoLog(string operationName, object[] inputs, bool isBefore = true)
        {
            OperationContext operationContext = OperationContext.Current;
            ServiceSecurityContext securityContext = ServiceSecurityContext.Current;

            string user = null;

            if (securityContext != null)
            {
                user = securityContext.PrimaryIdentity.Name;
            }

            string instanceType = operationContext.InstanceContext
                .GetServiceInstance().GetType().Name;
            string sessionId = operationContext.SessionId;
            List<string> arguments = new List<string>();
            foreach (var input in inputs)
            {
                if (input is null)
                    arguments.Add("null");
                else if (input is User)
                {
                    arguments.Add(((User) input).Id.ToString());
                    arguments.Add(((User) input).UserName);
                    arguments.Add(((User) input).IsAdmin.ToString());
                }
                else if (input is AmdbMetadata)
                {
                    arguments.Add(((AmdbMetadata) input).Id.ToString());
                    arguments.Add(((AmdbMetadata) input).Identifier.ToString());
                    arguments.Add(((AmdbMetadata) input).Name);
                }
                else
                    arguments.Add(input.ToString());
            }

            string result = String.Join(",", arguments);
            Logging.LogManager.GetLogger(instanceType).Info($"{user} | {sessionId} | {operationName}({result}) is " +
                                                    (isBefore ? "started" : "finished"));
        }

        #endregion
    }

    public class CustomParameterInspectorAttribute : Attribute, IOperationBehavior
    {
        private readonly int _index = 0;

        public CustomParameterInspectorAttribute() : this(0) { }

        private CustomParameterInspectorAttribute(int index)
        {
            _index = index;
        }

        #region IOperationBehavior Implementation
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            clientOperation.ParameterInspectors.Add(new CustomParameterInspector(_index));
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            dispatchOperation.ParameterInspectors.Add(new CustomParameterInspector(_index));
        }

        public void Validate(OperationDescription operationDescription)
        {
        }
        #endregion
    }
}
