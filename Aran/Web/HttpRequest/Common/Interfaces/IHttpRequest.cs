using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest.Interfaces
{
    public interface IHttpRequest
    {
        Task<HttpResponseMessage> Get(string requestUri, string accessToken = null);

        Task<HttpResponseMessage> Post(string requestUri, object value, string accessToken = null);

        Task<HttpResponseMessage> Put(string requestUri, object value, string accessToken = null);

        Task<HttpResponseMessage> Patch(string requestUri, object value, string accessToken = null);

        Task<HttpResponseMessage> Delete(string requestUri, string accessToken = null);
    }
}
