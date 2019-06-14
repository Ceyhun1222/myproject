using Microsoft.Extensions.Logging;
using OmsApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OmsApi.Interfaces
{
    public interface IOmegaServiceClient
    {
        Task<IList<RequestReportDto>> CheckRequest(RequestCheckDto requestData,ILogger logger);
    }    
}
