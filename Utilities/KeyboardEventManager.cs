using System;
using System.Windows.Input;

namespace Page_Navigation_App.Utilities;

/// <summary>
/// Utility class for handling keyboard events.
/// </summary>
public static class KeyboardEventManager
{
    /// <summary>
    /// Event raised when a key event is received.
    /// </summary>
    public static event EventHandler<KeyEventArgs> KeyEventReceived;

    /// <summary>
    /// Checks if a combination of keys is currently pressed.
    /// </summary>
    /// <param name="keys">The keys to check.</param>
    /// <returns>True if all the specified keys are pressed; otherwise, false.</returns>
    public static bool AreKeysPressed(params Key[] keys)
    {
        foreach (var key in keys)
        {
            if (!Keyboard.IsKeyDown(key))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Checks if the Alt key is currently pressed.
    /// </summary>
    /// <returns>True if the Alt key is pressed; otherwise, false.</returns>
    public static bool IsAltKeyPressed()
    {
        return Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt);
    }

    /// <summary>
    /// Checks if the Ctrl key is currently pressed.
    /// </summary>
    /// <returns>True if the Ctrl key is pressed; otherwise, false.</returns>
    public static bool IsCtrlKeyPressed()
    {
        return Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl);
    }

    /// <summary>
    /// Checks if the Shift key is currently pressed.
    /// </summary>
    /// <returns>True if the Shift key is pressed; otherwise, false.</returns>
    public static bool IsShiftKeyPressed()
    {
        return Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
    }

    /// <summary>
    /// Handles the key event and raises the KeyEventReceived event.
    /// </summary>
    /// <param name="e">The KeyEventArgs containing the key event data.</param>
    /// <param name="isKeyDown">A boolean indicating whether the key event is a KeyDown event (true) or a KeyUp event (false).</param>
    public static void OnKeyEventReceived(KeyEventArgs e, bool isKeyDown)
    {
        KeyEventReceived?.Invoke(null, e);
    }

    /// <summary>
    /// Handles the KeyDown event of the window.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The KeyEventArgs containing the event data.</param>
    public static void Window_KeyDown(object sender, KeyEventArgs e)
    {
        OnKeyEventReceived(e, true);
    }

    /// <summary>
    /// Handles the KeyUp event of the window.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The KeyEventArgs containing the event data.</param>
    public static void Window_KeyUp(object sender, KeyEventArgs e)
    {
        OnKeyEventReceived(e, false);
    }
}