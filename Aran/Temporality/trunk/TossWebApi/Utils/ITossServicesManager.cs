using Aran.Temporality.Common.Aim.MetaData;
using Aran.Temporality.Common.Interface;

namespace TossWebApi.Utils
{
    public interface ITossServicesManager
    {
        bool Open();
        ITemporalityService<AimFeature> GetDefaultTemporalityService();
        INoAixmDataService GetDefaultNoAixmDataService();
    }
}