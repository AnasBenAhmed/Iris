using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Iris.Core;

namespace Iris.App.Converters;

public class FileNameToImageConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var path = Path.Combine(GalleryStore.DefaultRoot(), "images", value?.ToString() ?? "");
        if (!File.Exists(path)) return null;
        var img = new BitmapImage();
        img.BeginInit();
        img.CacheOption = BitmapCacheOption.OnLoad;
        img.DecodePixelWidth = 160;
        img.UriSource = new Uri(path);
        img.EndInit();
        img.Freeze();
        return img;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
