using Aran.Aim.AixmMessage;
using Aran.Aim.Data.Filters;
using Aran.Aim.Data.WebServices.AIXMRESTService.Config;
using Aran.Aim.Enums;
using Aran.Aim.Features;
using Aran.Panda.Common;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace Aran.Aim.Data.WebServices.AIXMRESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AIXMService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AIXMService.svc or AIXMService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AIXMService : IAIXMService
    {
        // private TimeSliceInterpretationType _lastInterpreationType;
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        private DatabaseConfig dbConfig = DatabaseConfig.Settings;
        private DbProvider dbProvider = null;
        private List<Exception> excList = new List<Exception>();


        public Stream GetSnapshot(List<FeatureType> types, DateTime date)
        {

            try {
                if (types == null || types.Count == 0)
                    return getSnapshot();
                return getSnapshot(types, date);
            }catch(Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new WebFaultException<String>(ex.Message, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private Stream getSnapshot()
        {
            return getSnapshot(Enum.GetValues(typeof(FeatureType)).Cast<FeatureType>().ToList<FeatureType>());
        }

        private Stream getSnapshot(List<FeatureType> featureTypes, DateTime date = default(DateTime))
        {

            Stream stream = new MemoryStream();
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
            try
            {
                Connect();
                CreateAIXMStream(stream, featureTypes, date);
                dbProvider.Close();
                stream.Position = 0L;
                return stream;


            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                if (dbProvider != null && dbProvider.State == System.Data.ConnectionState.Open)
                    dbProvider.Close();
                throw ex;
            }
        }



        private void Connect()
        {
            dbProvider = new Temporality.Provider.TemporalityProvider();
            dbProvider.CallSpecialMethod("SetApplicationEnvironment", null);

            dbProvider.Open(null);
        }

        public void CreateAIXMStream(Stream stream, List<FeatureType> featureTypes, DateTime date = default(DateTime))
        {

            var writer = CreateXmlWriter(stream);
            foreach (var featureType in featureTypes)
            {
                GettingResult data = null;
                if (date != default(DateTime))
                    data = dbProvider.GetVersionsOf((FeatureType)featureType, TimeSliceInterpretationType.SNAPSHOT, default(Guid), false, new TimeSliceFilter(date));
                else
                    data = dbProvider.GetVersionsOf((FeatureType)featureType, TimeSliceInterpretationType.SNAPSHOT);
                if (data != null && data.IsSucceed)
                {
                    foreach (var item in data.GetListAs<Feature>())
                    {
                        WriteFeature(writer, item);
                    }
                }

            }
            CloseXmlWriter(writer);
        }


        private string ToJson(object value)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            return JsonConvert.SerializeObject(value, Newtonsoft.Json.Formatting.Indented, settings);
        }

        private void WriteFeature(XmlWriter writer, Feature feature)
        {
            try
            {
                writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "hasMember");
                {
                    var afl = new AixmFeatureList { feature };
                    afl.WriteXml(writer,
                        false,//WriteExtension
                        false,//Write3DCoordinateIfExists, 
                        SrsNameType.CRS84);
                }
                writer.WriteEndElement();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw ex;
            }
        }

        private XmlWriter CreateXmlWriter(Stream stream)
        {
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true };

            //stringBuilder = new StringBuilder();
            //var writer = XmlWriter.Create(fileName, settings);
            var writer = XmlWriter.Create(stream, settings);

            writer.WriteStartElement(AimDbNamespaces.AIXM51Message, "AIXMBasicMessage");
            writer.WriteAttributeString(AimDbNamespaces.XSI, "schemaLocation",
                "http://www.aixm.aero/schema/5.1/message http://www.aixm.aero/schema/5.1/message/AIXM_BasicMessage.xsd");
            writer.WriteAttributeString(AimDbNamespaces.GML, "id", CommonXmlWriter.GenerateNewGmlId());
            writer.WriteAttributeString("xmlns", AimDbNamespaces.AIXM51.Prefix, null, AimDbNamespaces.AIXM51.Namespace);
            if (false) //TODO:add metadata
            {
                writer.WriteStartElement(AimDbNamespaces.AIXM51, "messageMetadata");
                {
                }
                writer.WriteEndElement();
            }
            return writer;
        }

        private static void CloseXmlWriter(XmlWriter writer)
        {
            writer.WriteEndElement();
            writer.Flush();
            writer.Close();
        }

        public Stream GetSnapshotByFeturesAndDate(string types, string time)
        {
            throw new NotImplementedException();
        }
    }

}
