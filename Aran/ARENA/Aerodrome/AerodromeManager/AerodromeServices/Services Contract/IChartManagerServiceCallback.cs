using AerodromeServices.DataContract;
using System;
using System.ServiceModel;

namespace AerodromeServices.Services_Contract
{
    public interface IAmdbManagerServiceCallback
    {
        
        [OperationContract(IsOneWay = true)]
        void AmdbChanged(AmdbMetadata chart,AmdbCallBackType type);

        [OperationContract(IsOneWay = true)]
        void AllChartVersionsDeleted(Guid identifier);

        [OperationContract(IsOneWay = true)]
        void ChartsByEffectiveDateDeleted(Guid identifier, string version);

        [OperationContract(IsOneWay = true)]
        void UserChanged(UserCallbackType type);


    }
}
