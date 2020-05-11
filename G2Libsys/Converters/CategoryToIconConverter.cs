using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace G2Libsys.Converters
{
    public class CategoryToIconConverter : BaseValueConverter<CategoryToIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((int)value switch
            {
                1 => "Bookshelf",
                2 => "Books",
                3 => "Headphones",
                4 => "LocalMovies",
                5 => "EventWeek",
                _ => value.ToString()
            });
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
