using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Aim.Service;
using Aran.Temporality.Common.Config;
using Aran.Temporality.Common.Interface;
using Aran.Temporality.CommonUtil.Context;

namespace TossWebApi.Utils
{
    public class TossServicesManager : ITossServicesManager
    {
        public bool Open()
        {
            if (CurrentDataContext.CurrentService != null)
                return true;

            if (CurrentDataContext.Login())
            {
                ConfigUtil.ExternalApplication = "TossWebApi";
                ConfigUtil.ExternalApplicationUserName = CurrentDataContext.CurrentUser.Name;
                return true;
            }

            return false;
        }

        public ITemporalityService<AimFeature> GetDefaultTemporalityService()
        {
            if (!Open())
                throw new AccessViolationException("Cannot open connection!");
            
            return CurrentDataContext.CurrentService;
        }

        public INoAixmDataService GetDefaultNoAixmDataService()
        {
            if (!Open())
                throw new AccessViolationException("Cannot open connection!");

            return CurrentDataContext.CurrentNoAixmDataService;
        }
    }
}