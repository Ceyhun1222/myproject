using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aran.Aim
{
    public class DeserializedErrorInfo
    {
        public FeatureType FeatureType { get; set; }

        public Guid Identifier { get; set; }

        public string PropertyName { get; set; }

        public string ErrorMessage { get; set; }

        public string XmlMessage { get; set; }

        public string Action { get; set; }

        public CodeErrorType ErrorType { get; set; }

        
    }

    public enum CodeErrorType
    {
        Other,
        UnknownFeature,
        IdentifierDuplicated,
        GeometryNull,
        NonExistingReference

    }
}
