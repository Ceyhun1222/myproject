using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.IO;

namespace Aran.Aim.CAWProvider
{
    internal class ResponseReaderHelper
    {
        class RequestState
        {
            public static readonly int BUFFER_SIZE = 1024;
            public byte [] BufferRead = new byte [BUFFER_SIZE];
            public HttpWebRequest request;
            public HttpWebResponse response;
            public Stream streamResponse;
            public List<byte> requestBytes = new List<byte> ();
        }

        public ResponseReaderHelper ()
        {
            _allDone = new ManualResetEvent (false);
            DefaultTimeout = 2 * 60 * 1000; // 2 minutes timeout
        }

        public byte [] GetResponse (byte [] requestBytes, Uri server, byte [] authBytes)
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);

            var webReq = WebRequest.Create(server) as HttpWebRequest;
            webReq.Timeout = 400000;
            webReq.ProtocolVersion = HttpVersion.Version10;
            webReq.Headers[HttpRequestHeader.Authorization] = "Basic " + Convert.ToBase64String(authBytes);
            //webReq.Headers ["Authorization"] = "Basic " + Convert.ToBase64String (authBytes);
            //webReq.Credentials = new NetworkCredential("aimdb", "aimdb");

            webReq.Method = "POST";
            webReq.ContentType = "text/xml";
            webReq.ContentLength = requestBytes.Length;

            Stream requestStream = webReq.GetRequestStream ();
            requestStream.Write (requestBytes, 0, requestBytes.Length);
            requestStream.Close ();

            RequestState myRequestState = new RequestState ();
            myRequestState.request = webReq;

            // Start the asynchronous request.
            IAsyncResult result = (IAsyncResult) webReq.BeginGetResponse (
                new AsyncCallback (RespCallback), myRequestState);

            // this line implements the timeout, if there is a timeout, the callback fires and the request becomes aborted
            ThreadPool.RegisterWaitForSingleObject (result.AsyncWaitHandle,
                new WaitOrTimerCallback (TimeoutCallback), webReq, DefaultTimeout, true);

            // The response came in the allowed time. The work processing will happen in the 
            // callback function.
            _allDone.WaitOne ();

            if (_exception != null)
                throw _exception;

            // Release the HttpWebResponse resource.
            myRequestState.response.Close ();

            if (_exception != null)
                throw _exception;

            return myRequestState.requestBytes.ToArray ();
        }

        public bool AcceptAllCertifications(object sender, 
            System.Security.Cryptography.X509Certificates.X509Certificate certification, 
            System.Security.Cryptography.X509Certificates.X509Chain chain, 
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void TimeoutCallback (object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort ();
                    _exception = new Exception ("Timeout.");
                }
            }
        }

        private void RespCallback (IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState myRequestState = (RequestState) asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.request;
                myRequestState.response = (HttpWebResponse) myHttpWebRequest.EndGetResponse (asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = myRequestState.response.GetResponseStream ();
                myRequestState.streamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead (
                    myRequestState.BufferRead, 0, RequestState.BUFFER_SIZE,
                    new AsyncCallback (ReadCallBack), myRequestState);
                return;
            }
            catch (WebException e)
            {
                _exception = e;
            }
            _allDone.Set ();
        }

        private void ReadCallBack (IAsyncResult asyncResult)
        {
            try
            {
                RequestState myRequestState = (RequestState) asyncResult.AsyncState;
                Stream responseStream = myRequestState.streamResponse;
                int read = responseStream.EndRead (asyncResult);
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    for (int i = 0; i < read; i++)
                    {
                        myRequestState.requestBytes.Add (myRequestState.BufferRead [i]);
                    }

                    IAsyncResult asynchronousResult = responseStream.BeginRead (
                        myRequestState.BufferRead, 0, RequestState.BUFFER_SIZE,
                        new AsyncCallback (ReadCallBack), myRequestState);
                    return;
                }
                else
                {
                    responseStream.Close ();
                }

            }
            catch (Exception e)
            {
                _exception = e;
            }
            _allDone.Set ();
        }

        public int DefaultTimeout { get; set; }

        private ManualResetEvent _allDone;
        private Exception _exception;
    }
}
