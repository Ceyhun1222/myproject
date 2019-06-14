using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace Aran.Aim.Data.WebServices.AIXMRESTService
{
    public class TypeConverter : QueryStringConverter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        QueryStringConverter inner;
        public TypeConverter(QueryStringConverter inner)
        {
            this.inner = inner;
        }
        public override bool CanConvert(Type type)
        {
            return type == typeof(List<FeatureType>) || inner.CanConvert(type);
        }
        public override object ConvertStringToValue(string parameter, Type parameterType)
        {
            try {
                if (parameterType == typeof(List<FeatureType>))
                {
                    if (parameter == null)
                        return null;
                    string[] parts = parameter.Split(',');
                    List<FeatureType> result = new List<FeatureType>();
                    for (int i = 0; i < parts.Length; i++)
                        result.Add((FeatureType)Enum.Parse(typeof(FeatureType), parts[i]));
                    return result;
                }

                return inner.ConvertStringToValue(parameter, parameterType);
            }catch(Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new WebFaultException<string>(ex.Message, System.Net.HttpStatusCode.BadRequest);
            }

        }

        public override string ConvertValueToString(object parameter, Type parameterType)
        {
            try {
                if (parameterType == typeof(List<FeatureType>))
                {
                    if (parameter == null)
                        return null;
                    List<FeatureType> featureTypes = (List<FeatureType>)parameter;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < featureTypes.Count; i++)
                    {
                        if (i > 0) sb.Append(',');
                        sb.Append(featureTypes[i]);
                    }

                    return sb.ToString();
                }

                return inner.ConvertValueToString(parameter, parameterType);
            }
            catch(Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw new WebFaultException<string>(ex.Message, System.Net.HttpStatusCode.BadRequest);
            }
        }
    }
}