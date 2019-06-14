using System.Collections.Generic;
using System.ServiceModel;

namespace VisualizerCommon.Remote
{
    [ServiceContract]
    public interface IVisualizerServer
    {
        [OperationContract]
        void SetSelection(List<GeometrySelection> selection);

        [OperationContract]
        void ClearSelection();
    }
}
