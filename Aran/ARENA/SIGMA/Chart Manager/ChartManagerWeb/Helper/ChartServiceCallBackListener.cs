using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChartManagerWeb.ChartServiceReference;

namespace ChartManagerWeb.Helper
{
    public class ChartServiceCallBackListener : IChartManagerServiceCallback
    {
        public void ChartChanged(Chart chart, ChartCallBackType type)
        {
            //throw new NotImplementedException();
        }

        public void AllChartVersionsDeleted(Guid identifier)
        {
            //throw new NotImplementedException();
        }

        public void ChartsByEffectiveDateDeleted(Guid identifier, DateTime dateTime)
        {
            //throw new NotImplementedException();
        }

        public void UserChanged(UserCallbackType type)
        {
            //throw new NotImplementedException();
        }
    }
}