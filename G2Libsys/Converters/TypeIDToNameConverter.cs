using G2Libsys.Library;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace G2Libsys.Converters
{
    public class TypeIDToNameConverter : BaseValueConverter<TypeIDToNameConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value switch
            {
                1 => "Administratör",
                2 => "Bibliotekarie",
                3 => "Användare",
                _ => value.ToString()
            });

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
