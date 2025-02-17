﻿using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace UI.Converters;

public class InverseBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !(bool)value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return !(bool)value;
    }
}