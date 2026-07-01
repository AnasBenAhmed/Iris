using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Iris.App.Converters;

/// Visible when the bound count is greater than zero.
public class CountToVis : IValueConverter
{
    public static readonly CountToVis Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is int n && n > 0 ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

/// Visible when the bound value is null (used for empty-state placeholders).
public class NullToVis : IValueConverter
{
    public static readonly NullToVis Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is null ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
