using System;

namespace Dandraka.XmlUtilities
{
    public class ValueConversionException : Exception
    {
        public ValueConversionException(Type t, string value) : base($"Cannot convert {value} to type {t.FullName}") { }
    }
}
