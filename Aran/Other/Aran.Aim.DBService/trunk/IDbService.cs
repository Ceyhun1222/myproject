using System;
using System.ServiceModel;

namespace Aran.Aim.DBService
{
    [ServiceContract (Namespace = "http://Aran.Aim.DBService", Name = "IDbService")]
    public interface IDbService
    {
        [OperationContract]
        string SetFeatures (string aixmBasicMessageText);

        [OperationContract]
        string GetFeatures (DateTime creationTime);
    }
}
