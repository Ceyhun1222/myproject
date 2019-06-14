using HttpRequest.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpRequest.Implementations
{
    public class JsonHttpRequest : IHttpRequest
    {
        public async Task<HttpResponseMessage> Get(string requestUri, string accessToken = null)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Get)
                .AddRequestUri(requestUri);

            if (accessToken != null)
                builder.AddAccessToken(accessToken);

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Post(string requestUri, object value, string accessToken = null)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Post)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            if (accessToken != null)
                builder.AddAccessToken(accessToken);

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Put(string requestUri, object value, string accessToken = null)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Put)
                .AddRequestUri(requestUri)
                .AddContent(new JsonContent(value));

            if (accessToken != null)
                builder.AddAccessToken(accessToken);

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Patch(string requestUri, object value, string accessToken = null)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(new HttpMethod("PATCH"))
                .AddRequestUri(requestUri)
                .AddContent(new PatchContent(value));

            if (accessToken != null)
                builder.AddAccessToken(accessToken);

            return await builder.SendAsync();
        }

        public async Task<HttpResponseMessage> Delete(string requestUri, string accessToken = null)
        {
            var builder = new HttpRequestBuilder()
                .AddMethod(HttpMethod.Delete)
                .AddRequestUri(requestUri);

            if (accessToken != null)
                builder.AddAccessToken(accessToken);

            return await builder.SendAsync();
        }
    }
}
