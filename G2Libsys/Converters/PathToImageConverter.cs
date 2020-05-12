using System;
using System.Globalization;
using System.Windows.Media.Imaging;

namespace G2Libsys.Converters
{
    public class PathToImageConverter : BaseValueConverter<PathToImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            return new BitmapImage(new Uri(value.ToString(), UriKind.Absolute));
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
