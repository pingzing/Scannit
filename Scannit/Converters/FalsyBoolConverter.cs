using System;
using System.Collections;
using System.Globalization;
using Xamarin.Forms;

namespace Scannit.Converters
{
    public class FalsyBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return HasValueOrNonZero(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private bool HasValueOrNonZero(object value)
        {
            if (value == null)
            {
                return false;
            }

            if (value is bool boolVal)
            {
                return boolVal;
            }

            if (value is byte byteVal)
            {
                return byteVal != 0;
            }

            if (value is short shortVal)
            {
                return shortVal != 0;
            }

            if (value is int intVal)
            {
                return intVal != 0;
            }

            if (value is decimal decimalVal)
            {
                return decimalVal != 0;
            }

            if (value is long longVal)
            {
                return longVal != 0;
            }

            if (value is float floatVal)
            {
                return floatVal != 0;
            }

            if (value is double doubleVal)
            {
                return doubleVal != 0;
            }

            if (value is char charVal)
            {
                return !Char.IsWhiteSpace(charVal);
            }

            string stringVal = value as string;
            if (stringVal != null)
            {
                return !String.IsNullOrWhiteSpace(stringVal);
            }

            IEnumerable enumerableVal = value as IEnumerable;
            if (enumerableVal != null)
            {
                return enumerableVal.GetEnumerator().MoveNext();
            }

            return true;
        }
    }
}
