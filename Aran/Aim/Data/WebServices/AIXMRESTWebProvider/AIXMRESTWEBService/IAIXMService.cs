using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Aran.Aim.Data.WebServices.AIXMRESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IAIXMService" in both code and config file together.
    [ServiceContract]
    public interface IAIXMService
    {
        [OperationContract]
        [WebGet(UriTemplate = "Snapshot?types={types}&date={date}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Xml)]
        Stream GetSnapshot(List<FeatureType> types, DateTime date);
    }
}
