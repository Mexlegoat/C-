using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace GestionnaireApp
{
    [ValueConversion(typeof(string), typeof(BitmapSource))]
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = value as string;
            if (string.IsNullOrEmpty(path))
                return null;

            try
            {
                Icon icon = Icon.ExtractAssociatedIcon(path);
                return Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    System.Windows.Int32Rect.Empty,
                    BitmapSizeOptions.FromWidthAndHeight(32, 32));
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}