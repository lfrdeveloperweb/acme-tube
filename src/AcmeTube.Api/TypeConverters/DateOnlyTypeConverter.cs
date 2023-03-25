﻿using System;
using System.ComponentModel;
using System.Globalization;

namespace AcmeTube.Api.TypeConverters
{
    public class DateOnlyTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value is string str 
                ? DateOnly.Parse(str) 
                : base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return destinationType == typeof(string) && value is DateOnly date
                ? date.ToString("O")
                : base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
