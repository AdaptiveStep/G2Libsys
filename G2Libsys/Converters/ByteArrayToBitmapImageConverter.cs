using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace G2Libsys.Converters
{
	public class ByteArrayToBitmapImageConverter : BaseValueConverter<ByteArrayToBitmapImageConverter>
	{
		/// <summary>
		/// Converts "raw image data" (a binary blob) to a bitmapimage. Is used when downloading images.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var rawImageData = value as byte[];
			if (rawImageData == null)
				return null;

			var bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
			using (var stream = new MemoryStream(rawImageData))
			{
				bitmapImage.BeginInit();
				bitmapImage.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
				bitmapImage.CacheOption = BitmapCacheOption.Default;
				bitmapImage.StreamSource = stream;
				bitmapImage.EndInit();
			}
			return bitmapImage;
		}

		/// <summary>
		/// Currently not implemented.
		/// Is used to Convert Image back to a blob. (Is used to upload images). 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

	}
}
