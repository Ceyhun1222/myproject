using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Aran.Temporality.Internal.Remote.Service
{
    [ServiceContract]
    public interface IExternalSystemService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Dummy")] 
        string Dummy();

        [WebGet(UriTemplate = "Test")]
        Stream Test();
    }
}
