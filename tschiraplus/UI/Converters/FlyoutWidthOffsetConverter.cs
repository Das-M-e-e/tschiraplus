using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace UI.Converters;

public class FlyoutWidthOffsetConverter : IValueConverter
{
    private const double Offset = 80;

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double width)
        {
            return width - Offset;
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}