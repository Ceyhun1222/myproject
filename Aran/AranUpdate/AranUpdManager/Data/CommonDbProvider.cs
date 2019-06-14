using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace AranUpdater.CommonData
{
    public class CommonDbProvider
    {
        protected string _serverUrl;

        public bool IsOpen
        {
            get { return _serverUrl != null; }
        }


        protected void Open(string server, int port, string urlTemp)
        {
            _serverUrl = string.Format("http://{0}:{1}/{2}", server, port, urlTemp);
        }

        protected void GetResponse(string action, object input = null)
        {
            var stream = GetResponseStream(action, input, "POST");

            var ser = new DataContractJsonSerializer(typeof(AResponse));
            var aResponse = ser.ReadObject(stream) as AResponse;

            if (!aResponse.IsOk)
                throw new Exception(aResponse.ErrorMessage);
        }

        protected T GetResponse<T>(string action, object input = null)
        {
            var stream = GetResponseStream(action, input, "POST");

            var ser = new DataContractJsonSerializer(typeof(AResponse<T>));
            var aResponse = ser.ReadObject(stream) as AResponse<T>;

            if (!aResponse.IsOk)
                throw new Exception(aResponse.ErrorMessage);

            return aResponse.Result;
        }

        protected void GetResponseGET(string action)
        {
            var stream = GetResponseStream(action, null, "GET");

            var ser = new DataContractJsonSerializer(typeof(AResponse));
            var aResponse = ser.ReadObject(stream) as AResponse;

            if (!aResponse.IsOk)
                throw new Exception(aResponse.ErrorMessage);
        }

        protected T GetResponseGET<T>(string action)
        {
            var stream = GetResponseStream(action, null, "GET");

            var ser = new DataContractJsonSerializer(typeof(AResponse<T>));
            var aResponse = ser.ReadObject(stream) as AResponse<T>;

            if (!aResponse.IsOk)
                throw new Exception(aResponse.ErrorMessage);

            return aResponse.Result;
        }

        protected Stream GetResponseStream(string action, object input, string method)
        {
            var req = HttpWebRequest.Create(_serverUrl + "/" + action);
            req.Method = method;
            req.ContentType = "application/json";

            if (input != null)
            {
                var reqStream = req.GetRequestStream();
                var srl = new DataContractJsonSerializer(input.GetType());
                srl.WriteObject(reqStream, input);
            }

            var webResponse = req.GetResponse();
            return webResponse.GetResponseStream();
        }
    }
}
