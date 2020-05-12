using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace G2Libsys.Converters
{
    /// <summary>
    /// Base class for value converters
    /// </summary>
    /// <typeparam name="T">Converter class</typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T : class, new()
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly T _converter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        public override object ProvideValue(IServiceProvider provider)
        {
            return _converter ?? new T();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}
