using System.Windows.Input;

namespace Page_Navigation_App.Utilities;

public static class KeyEventArgsConverter
{
    public enum PlayingHand
    {
        Left,
        Right
    }

    public static string ConvertKey(KeyEventArgs e, PlayingHand playingHand)
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
}