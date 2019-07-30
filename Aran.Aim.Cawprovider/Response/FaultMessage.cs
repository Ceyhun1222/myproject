using Aran.Aim.AixmMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Aran.Aim.CAWProvider
{
    public class FaultMessage : AbstractResponse
    {
        public FaultMessage ()
        {
            ErrorMessageParamaters = new List<string> ();
        }

        public string Type { get; set; }
        public string ErrorMessageCode { get; set; }
        public LogMessageCategoryType Category { get; set; }
        public List<string> ErrorMessageParamaters { get; private set; }
        public string DefaultErrorMessage { get; set; }

        public override void ReadXml (XmlReader reader)
        {
            int depth = reader.Depth;

            while (reader.Read ())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "type")
                    {
                        Type = reader.ReadString ();
                    }
                    else if (reader.LocalName == "errorMessageCode")
                    {
                        ErrorMessageCode = reader.ReadString ();
                    }
                    else if (reader.LocalName == "category")
                    {
                        Category = CommonXmlReader.ParseEnum<LogMessageCategoryType>(reader.ReadString());
                    }
                    else if (reader.LocalName == "errorMessageParameter")
                    {
                        string errMsgParam = reader.ReadString ();
                        ErrorMessageParamaters.Add (errMsgParam);
                    }
                    else if (reader.LocalName == "defaultErrorMessage")
                    {
                        DefaultErrorMessage = reader.ReadString ();
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Depth == depth)
                {
                    break;
                }
            }
        }

        public override string ToString()
        {
            var s = string.Empty;

            if (!string.IsNullOrWhiteSpace(Type))
                s += "Type: " + Type;
            
            if (!string.IsNullOrWhiteSpace(ErrorMessageCode))
                s += "\r\nCode: " + ErrorMessageCode;

            if (ErrorMessageParamaters.Count > 0)
                s += "\r\nParameters: " + string.Join<string>("\r\n", ErrorMessageParamaters.Where(emp => !string.IsNullOrWhiteSpace(emp)));

            if (!string.IsNullOrWhiteSpace(DefaultErrorMessage))
                s += "\r\n" + DefaultErrorMessage;

            if (s == string.Empty)
                return base.ToString();

            return s; 
        }
    }
}
