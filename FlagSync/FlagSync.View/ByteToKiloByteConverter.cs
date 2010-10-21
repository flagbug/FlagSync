using System;
using System.Globalization;
using System.Windows.Data;

namespace FlagSync.View
{
    [ValueConversion(typeof(long), typeof(int))]
    public class ByteToKiloByteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long bytes = (long)value;
            return (int)(bytes / 1024);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int kiloBytes = (int)value;
            return (long)(kiloBytes * 1024);
        }
    }
}
