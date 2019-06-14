using System;
using System.Collections.Generic;
using Telerik.WinControls.Enumerations;
using System.ComponentModel;
using System.Globalization;

namespace AIP.DB
{

    public class ArrayToStringTypeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string[] values = (string[])value;
            return string.Join(",", values);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string[]))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string values = (string)value;
            return values.Split(',');
        }
    }

    public class YesNoConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(ToggleState);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            YesNo YesNoValue = (YesNo)value;

            switch (YesNoValue)
            {
                case YesNo.Yes:
                    return ToggleState.On;
                case YesNo.No:
                    return ToggleState.Off;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(ToggleState);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            ToggleState state = (ToggleState)value;

            switch (state)
            {
                case ToggleState.On:
                    return YesNo.Yes;
                case ToggleState.Off:
                    return YesNo.No;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }

}
