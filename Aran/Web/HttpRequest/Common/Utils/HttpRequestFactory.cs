using HttpRequest.Implementations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest.Utils
{
    public static class HttpRequestFactory
    {
        private static JsonHttpRequest _httpRequest = new JsonHttpRequest();

        public static async Task<HttpResponseMessage> Get(string requestUri, string accessToken = null)
        {
            return await _httpRequest.Get(requestUri, accessToken);
        }

        public static async Task<HttpResponseMessage> Post(string requestUri, object value, string accessToken = null)
        {
            return await _httpRequest.Post(requestUri, value, accessToken);
        }

        public static async Task<HttpResponseMessage> Put(string requestUri, object value, string accessToken = null)
        {
            return await _httpRequest.Put(requestUri, value, accessToken);
        }

        public static async Task<HttpResponseMessage> Patch(string requestUri, object value, string accessToken = null)
        {
            return await _httpRequest.Patch(requestUri, value, accessToken);
        }

        public static async Task<HttpResponseMessage> Delete(string requestUri, string accessToken = null)
        {
            return await _httpRequest.Delete(requestUri, accessToken);
        }
    }
}
