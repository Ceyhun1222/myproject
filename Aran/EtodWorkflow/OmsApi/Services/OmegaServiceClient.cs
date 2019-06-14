using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OmsApi.Configuration;
using OmsApi.Dto;
using OmsApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace OmsApi.Services
{
    public class OmegaServiceClient : IOmegaServiceClient
    {
        private HttpClient _httpClient;

        public OmegaServiceClient(HttpClient httpclient, OmegaServiceConfig options)
        {
            _httpClient = httpclient;
            _httpClient.BaseAddress = new Uri(options.Url);
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IList<RequestReportDto>> CheckRequest(RequestCheckDto requestData,ILogger logger)
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/obstacle-checker/surfaces", requestData);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Omega Check Request error => {response.RequestMessage}");
                throw;
            }            
            var result = await response.Content.ReadAsAsync<IList<RequestReportDto>>();
            return result;
        }
    }
}