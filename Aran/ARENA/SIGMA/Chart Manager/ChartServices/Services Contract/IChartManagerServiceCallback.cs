using ChartServices.DataContract;
using System;
using System.ServiceModel;

namespace ChartServices.Services_Contract
{
    public interface IChartManagerServiceCallback
    {
        
        [OperationContract(IsOneWay = true)]
        void ChartChanged(Chart chart,ChartCallBackType type);

        [OperationContract(IsOneWay = true)]
        void AllChartVersionsDeleted(Guid identifier);

        [OperationContract(IsOneWay = true)]
        void ChartsByEffectiveDateDeleted(Guid identifier, DateTime dateTime);

        [OperationContract(IsOneWay = true)]
        void UserChanged(UserCallbackType type);


    }
}
