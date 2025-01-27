using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace UI.Converters;

public class FlyoutOffsetConverter : IValueConverter
{
    private const double Offset = 80;
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double bounds)
        {
            return bounds - Offset;
        }

        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}