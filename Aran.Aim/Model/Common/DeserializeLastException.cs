using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Aran.Aim
{
    internal static class DeserializeLastException
    {
        private static List<string> _propertyPathList;


        static DeserializeLastException()
        {
            _propertyPathList = new List<string>();
            ErrorInfoList = new List<DeserializedErrorInfo>();
        }


        public static Exception Exception { get; set; }

        public static ReadOnlyCollection<string> PropertyPathList
        {
            get { return new ReadOnlyCollection<string>(_propertyPathList); }
        }

        public static string MakeFullPropPath()
        {
            return string.Join<string>("/", _propertyPathList);
        }

        public static void ClearPropertyPath()
        {
            _propertyPathList.Clear();
        }

        public static void AddPropName(string propName)
        {
            _propertyPathList.Add(propName);
        }

        public static void RemoveLastPropName()
        {
            if (_propertyPathList.Count > 0)
                _propertyPathList.RemoveAt(PropertyPathList.Count - 1);
        }

        public static void ReplaceLastPropName(string propName)
        {
            RemoveLastPropName();
            if (propName.Length > 0)
                AddPropName(propName);
        }

        public static bool HasError
        {
            get { return (Exception != null); }
        }

        public static DeserializedErrorInfo LastErrorInfo { get; set; }

        public static void AddErrorInfo()
        {
            ErrorInfoList.Add(new DeserializedErrorInfo
            {
                FeatureType = LastErrorInfo.FeatureType,
                Identifier = LastErrorInfo.Identifier,
                PropertyName = string.Join<string>("/", _propertyPathList),
                ErrorMessage = Exception == null ? null : Exception.Message,
                XmlMessage = LastErrorInfo.XmlMessage
            });

            Exception = null;
        }

        public static void AddWarningInfo(string message)
        {
            //ErrorInfoList.Add(new DeserializedErrorInfo
            //{
            //    FeatureType = LastErrorInfo.FeatureType,
            //    Identifier = LastErrorInfo.Identifier,
            //    PropertyName = string.Join<string>("/", _propertyPathList),
            //    ErrorMessage = Exception == null ? null : Exception.Message,
            //    XmlMessage = LastErrorInfo.XmlMessage
            //});

            //Exception = null;
        }

        public static List<DeserializedErrorInfo> ErrorInfoList { get; private set; }
    }
}
