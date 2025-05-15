using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Menu_Restaurant_2.Converters
{
    public class ImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fileName = value as string;

            if (string.IsNullOrEmpty(fileName))
                return new BitmapImage(new Uri("pack://application:,,,/Resources/default.png"));

            return new BitmapImage(new Uri($"pack://application:,,,/Resources/{fileName}"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
