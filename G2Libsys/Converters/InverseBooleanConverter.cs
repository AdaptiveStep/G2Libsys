using System;
using System.Windows.Data;

namespace G2Libsys.Converters
{
    //TODO DELETE THIS FILE
    /// <summary>
    /// Is used by AdvancedSearch to collapse when Normal Searchbar is showed.
    /// Currently not implemented
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InverseBooleanConverter : BaseValueConverter<InverseBooleanConverter>
    {
        #region IValueConverter Members

        public override object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            

            return !(bool)value;
        }

        public override object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        #endregion
    }
}
