using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            SendXml();
        }

        private static void SendXml()
        {
            HttpWebRequest req = null;
            HttpWebResponse resp = null;

            string baseAddress = "http://localhost/AIXMWebService/AIXMService.svc";

            try
            {
                req = (HttpWebRequest)WebRequest.Create(baseAddress + "/Snapshot");

                req.Method = "GET";
                req.Timeout = 600000;



                resp = req.GetResponse() as HttpWebResponse;
                Console.WriteLine("HTTP/{0} {1} {2}",
                resp.ProtocolVersion, (int)resp.StatusCode, resp.StatusDescription);

                Stream sr = resp.GetResponseStream();
                using (FileStream file = new FileStream("res.xml", FileMode.Create, System.IO.FileAccess.Write))
                {
                    int count = 0;
                    do
                    {
                        byte[] buf = new byte[1024];
                        count = sr.Read(buf, 0, 1024);
                        file.Write(buf, 0, count);
                    } while (sr.CanRead && count > 0);
                }
                Console.WriteLine("END");
                sr.Close();
            }
            catch (WebException webEx)
            {
                Console.WriteLine("Web exception :" + webEx.Message);
                Console.WriteLine(webEx.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception :" + ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                //if (req != null)
                //    req.GetRequestStream().Close();
                //if (resp != null)
                //    resp.GetResponseStream().Close();
            }
        }
        private static string GetTextFromXMLFile(string file)
        {
            StreamReader reader = new StreamReader(file);
            string ret = reader.ReadToEnd();
            reader.Close();
            return ret;
        }
    }
}
