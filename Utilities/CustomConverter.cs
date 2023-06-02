using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Page_Navigation_App.Utilities;

public class CustomConverter : ICustomConverter
{
    public enum PlayingHand
    {
        Left,
        Right
    }

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

    object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    object ICustomConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotImplementedException();

    /*    public string ConvertKey(KeyEventArgs e, PlayingHand playingHand = PlayingHand.Right)
        {
            // Implementiere hier die Logik zur Konvertierung von KeyEventArgs in einen Notennamen
            // basierend auf dem übergebenen PlayingHand-Parameter
            // Rückgabe des entsprechenden Notennamens

            return "";
        }*/

    string ICustomConverter.ConvertKey(KeyEventArgs e, PlayingHand playingHand)
    {
        var convertedKey = string.Empty;

        switch (playingHand)
        {
            case PlayingHand.Left:
                convertedKey = ConvertLeftHandKey(e.Key);
                break;

            case PlayingHand.Right:
                convertedKey = ConvertRightHandKey(e.Key);
                break;
        }

        return convertedKey;
    }

    private static string ConvertLeftHandKey(Key key)
    {
        var convertedKey = string.Empty;

        switch (key)
        {
            case Key.F5:
            case Key.C:
                convertedKey = "C";
                break;

            case Key.F6:
            case Key.V:
                convertedKey = "D";
                break;

            case Key.F7:
            case Key.B:
                convertedKey = "E";
                break;

            case Key.F8:
            case Key.N:
                convertedKey = "F";
                break;

            case Key.F9:
            case Key.M:
                convertedKey = "G";
                break;
        }

        return convertedKey;
    }

    private static string ConvertRightHandKey(Key key)
    {
        var convertedKey = string.Empty;

        switch (key)
        {
            case Key.F4:
            case Key.Space:
            case Key.C:
                convertedKey = "C";
                break;

            case Key.F5:
            case Key.V:
                convertedKey = "D";
                break;

            case Key.F6:
            case Key.B:
                convertedKey = "E";
                break;

            case Key.F7:
            case Key.N:
                convertedKey = "F";
                break;

            case Key.F8:
            case Key.M:
                convertedKey = "G";
                break;
        }

        return convertedKey;
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