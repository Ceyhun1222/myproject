using GeoAPI.CoordinateSystems.Transformations;
using System.Threading.Tasks;

namespace ObstacleChecker.API.Utils
{
    public interface IMathTranformCreater
    {
        Task<IMathTransform> CreateTransformOnAdhpAsync(string adhpName, int workPackage);
    }
}
