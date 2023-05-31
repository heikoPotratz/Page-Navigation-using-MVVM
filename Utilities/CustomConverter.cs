using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Page_Navigation_App.Utilities;

public class CustomConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Implementieren Sie hier Ihre verschiedenen Konvertierungsfunktionen
        if (parameter is string function)
        {
            switch (function)
            {
                case "DoubleToGridLength":
                    return DoubleToGridLength(value);

                case "OtherFunction":
                    return OtherFunctionConvert(value);
                // Weitere Funktionen hier hinzufügen
                default:
                    throw new NotSupportedException($"Function '{function}' not supported.");
            }
        }
        throw new ArgumentException("Invalid parameter type.");
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    // Beispielkonvertierungsfunktionen
    private object DoubleToGridLength(object value)
    {
        var borderWidth = (double)value;
        return new GridLength(borderWidth);
    }

    private object OtherFunctionConvert(object value)
    {
        // Implementieren Sie Ihre andere Konvertierungsfunktion hier
        return null;
    }
}