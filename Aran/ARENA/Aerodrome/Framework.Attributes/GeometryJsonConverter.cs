using ESRI.ArcGIS.esriSystem;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    public class GeometryJsonConverter : JsonConverter
    {
        public static byte[] SetObjectToBlob(object SHP, string propertyName)
        {
            // вначале переведем IGeometry к типу IMemoryBlobStream 
            IMemoryBlobStream memBlb = new MemoryBlobStream();
            IObjectStream objStr = new ObjectStream();
            objStr.Stream = memBlb;
            ESRI.ArcGIS.esriSystem.IPropertySet propertySet = new ESRI.ArcGIS.esriSystem.PropertySetClass();
            IPersistStream perStr = (IPersistStream)propertySet;
            propertySet.SetProperty(propertyName, SHP);
            perStr.Save(objStr, 0);

            ////затем полученный IMemoryBlobStream представим в виде массива байтов
            object o;
            ((IMemoryBlobStreamVariant)memBlb).ExportToVariant(out o);

            byte[] bytes = (byte[])o;


            return bytes;
        }

        public static object GetObjectFromBlob(object anObject, string propName)
        {

            try
            {
                byte[] bytes = (byte[])anObject;
                // сконвертируем его в геометрию 
                IMemoryBlobStream memBlobStream = new MemoryBlobStream();

                IMemoryBlobStreamVariant varBlobStream = (IMemoryBlobStreamVariant)memBlobStream;

                varBlobStream.ImportFromVariant(bytes);

                IObjectStream anObjectStream = new ObjectStreamClass();
                anObjectStream.Stream = memBlobStream;

                IPropertySet aPropSet = new PropertySetClass();

                IPersistStream aPersistStream = (IPersistStream)aPropSet;
                aPersistStream.Load(anObjectStream);

                object result = aPropSet.GetProperty(propName);

                return result;

            }
            catch
            {
                return null;
            }
        }


        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string propName = "geo";
            byte[] bytes = SetObjectToBlob(value, propName);
            writer.WriteValue(Convert.ToBase64String(bytes));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string propName = "geo";
            if (reader.TokenType == JsonToken.Null)
                return null;

            var m = Convert.FromBase64String((string)reader.Value);
            var decode = GetObjectFromBlob(m, propName);
            return decode;
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }
    }
}
