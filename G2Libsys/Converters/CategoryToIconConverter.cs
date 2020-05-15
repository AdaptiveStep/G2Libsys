using System;
using System.Globalization;

namespace G2Libsys.Converters
{
    public class CategoryToIconConverter : BaseValueConverter<CategoryToIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value switch
            {
                0 => "EventWeek",
                1 => "Bookshelf",
                2 => "Books",
                3 => "Headphones",
                4 => "LocalMovies",
                _ => "RomanNumeral" + value
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
