using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Net;
using Aran.PANDA.Common;
using Aran.Aim.AixmMessage;

namespace Aran.Aim.CAWProvider
{
    public class CsCawPortTypeService
    {
        public CsCawPortTypeService()
        {
            _exceptionList = new List<Exception>();
            UseChache = false;
        }


        public ConnectionInfo ConnectionInfo
        {
            get
            {
                return _connectionInfo;
            }
            set
            {
                _connectionInfo = value;

                string auth = string.Format("{0}:{1}", value.UserName, value.Password);
                _authBytes = Encoding.UTF8.GetBytes(auth);

                (_connectionInfo as System.ComponentModel.INotifyPropertyChanged).PropertyChanged += ConnectionInfo_PropertyChanged;
            }
        }

        public TransactionResponse Transaction(Transaction transacion)
        {
            AimSerializableErrorHandler.SerializableEvent += new SerializableEventHandler(Serializable_ErrorEvent);
            byte[] requestBytes = GetSoapEnvBytes(transacion);

            using (XmlReader reader = GetResponse(requestBytes))
            {

                TransactionResponse response = null;

                if (reader.ReadToFollowing("Body", AimDbNamespaces.SoapEnv.Namespace))
                {
                    if (CommonXmlReader.MoveInnerElement(reader))
                    {
                        if (reader.LocalName == "TransactionResponse")
                        {
                            response = new TransactionResponse();
                            response.ReadXml(reader);
                        }
                    }
                }

                AimSerializableErrorHandler.SerializableEvent -= new SerializableEventHandler(Serializable_ErrorEvent);
                return response;
            }
        }

        public GetFeatureResponse GetFeature(GetFeature getFeature)
        {
            AimSerializableErrorHandler.SerializableEvent += new SerializableEventHandler(Serializable_ErrorEvent);
            byte[] requestBytes = GetSoapEnvBytes(getFeature);

            using (XmlReader reader = GetResponse(requestBytes))
            {
                GetFeatureResponse response = null;

                if (reader.ReadToFollowing("Body", AimDbNamespaces.SoapEnv.Namespace))
                {
                    if (CommonXmlReader.MoveInnerElement(reader))
                    {
                        if (reader.LocalName == "GetFeatureResponse")
                        {
                            response = new GetFeatureResponse();
                            response.ReadXml(reader);
                        }
                    }
                }

                AimSerializableErrorHandler.SerializableEvent -= new SerializableEventHandler(Serializable_ErrorEvent);
                return response;
            }
        }

        public TResponse GetResponse<TResponse>(AbstractRequest request) where TResponse : AbstractResponse, new()
        {
            byte[] requestBytes = GetSoapEnvBytes(request);
            using (XmlReader reader = GetResponse(requestBytes))
            {
                if (reader.ReadToFollowing("Body", AimDbNamespaces.SoapEnv.Namespace))
                {
                    if (CommonXmlReader.MoveInnerElement(reader))
                    {
                        if (reader.LocalName == typeof(TResponse).Name)
                        {
                            TResponse res = new TResponse();
                            res.ReadXml(reader);
                            return res;
                        }
                    }
                }
            }

            return null;
        }

        public bool UseChache { get; set; }


        private byte[] GetSoapEnvBytes(AbstractRequest request)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings sett = new XmlWriterSettings();
            sett.Indent = true;
            XmlWriter writer = XmlWriter.Create(ms, sett);

            writer.WriteStartDocument();
            {
                writer.WriteStartElement("soapenv", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
                {
                    writer.WriteElementString("soapenv", "Header", null, null);

                    writer.WriteStartElement("soapenv", "Body", null);
                    {
                        request.WriteXml(writer);
                    }
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
            }
            writer.WriteEndDocument();

            writer.Close();
            writer.Flush();

            byte[] buffer = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        private XmlReader GetResponse(byte[] requestBytes)
        {
			//string fileFullName = null;
			//if (UseChache)
			//{
			//    XmlReader cacheReader = GetCacheReader (requestBytes, out fileFullName);
			//    if (cacheReader != null)
			//        return cacheReader;
			//}

			var readerHelper = new ResponseReaderHelper();
            byte[] responseBytes = readerHelper.GetResponse(requestBytes, _connectionInfo.Server, _authBytes);

            if (WriteReqRespData)
            {
                WriteReqRespData = false;

                var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dir = Path.Combine(dir, "RISK", "TEST");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                var now = DateTime.Now;
                var filePath = Path.Combine(dir, "req_" + now.ToString("yyyyMMddHHmmss") + ".xml");
                File.WriteAllBytes(filePath, requestBytes);

                filePath = Path.Combine(dir, "resp_" + now.ToString("yyyyMMddHHmmss") + ".xml");
                File.WriteAllBytes(filePath, responseBytes);
            }

			var ms = new MemoryStream(responseBytes);
            return XmlReader.Create(ms);
        }

        private XmlReader GetCacheReader(byte[] requestBytes, out string fileFullName)
        {
            string conStr = _connectionInfo.Server + _connectionInfo.UserName;
            string reqStr = UTF8Encoding.UTF8.GetString(requestBytes);
            string str = conStr + reqStr;
            var fileName = CRC32.CalcCRC32(str);

            //if (buffer.Length > 256)
            //{
            //    if (buffer.Length > 512)
            //    {
            //        fileName = CRC32.CalcCRC32 (buffer, 0, 256);
            //        fileName += CRC32.CalcCRC32 (buffer, 256, 512);
            //        fileName += CRC32.CalcCRC32 (buffer, 512);
            //    }
            //    else
            //    {
            //        fileName = CRC32.CalcCRC32 (buffer, 0, 256);
            //        fileName += CRC32.CalcCRC32 (buffer, 256);
            //    }
            //}
            //else
            //{
            //    fileName = CRC32.CalcCRC32 (buffer);
            //}

            string cachePath = System.Environment.GetEnvironmentVariable("TMP");

            cachePath = Path.Combine(cachePath, "PandaCawCache");

            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);

            fileFullName = Path.Combine(cachePath, fileName + ".xml");

            if (!File.Exists(fileFullName))
                return null;

            return XmlReader.Create(fileFullName);
        }

        private void Serializable_ErrorEvent(object sender, Exception ex)
        {
            _exceptionList.Add(ex);
        }

        private void ConnectionInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "UserName" || e.PropertyName == "Password")
            {
                var connInfo = sender as ConnectionInfo;
                string auth = string.Format("{0}:{1}", connInfo.UserName, connInfo.Password);
                _authBytes = Encoding.UTF8.GetBytes(auth);
            }
        }

        public byte[] Test(byte[] requestBytes)
        {
            var readerHelper = new ResponseReaderHelper();
            return readerHelper.GetResponse(requestBytes, _connectionInfo.Server, _authBytes);
        }

        private ConnectionInfo _connectionInfo;
        private byte[] _authBytes;
        private List<Exception> _exceptionList;

#warning FOR_DEBUG
		public static bool WriteReqRespData = false;
	}
}
