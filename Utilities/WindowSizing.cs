using System;
using System.Windows;
using System.Windows.Controls;

namespace Page_Navigation_App.Utilities;

internal static class WindowSizing
{
    private static readonly double DefaultWindowWidth = GetDefaultMainWindowWidth();
    private static readonly double NarrowWindowWidth = CalculateShrinkedMainWindowWidth();

    public static void ShrinkNavigationWidth()
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            // shrink NavigationBar
            mainWindow.NavColumn.Style = (Style)mainWindow.FindResource("NavColumnNarrowWidth_Style");
            // the MainWindow as well
            SetMainWindowWidth(NarrowWindowWidth);
        }
    }

    public static void ToggleNavigationWidth()
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            if (mainWindow.Width != DefaultWindowWidth)
            {
                RestoreNavigationWidth();
            }
            else
            {
                ShrinkNavigationWidth();
            }
        }
    }

    /// <summary>
    /// Calculate Windowwidth in Case of shrinked NavigationBar
    /// </summary>
    /// <returns></returns>
    private static double CalculateShrinkedMainWindowWidth()
    {
        var defaultNavColumn = GetWidthFromResource("NavColumn_Width");
        var defaultNavNarrowColumn = GetWidthFromResource("NavNarrowColumn_Width");

        return DefaultWindowWidth - (defaultNavColumn - defaultNavNarrowColumn);
    }

    private static ColumnDefinition FindColumnDefinitionInVisualTree(MainWindow mainWindow, string columnDefinitionStyleName)
    {
        var navColumn = (ColumnDefinition)mainWindow.FindName(columnDefinitionStyleName);
        return navColumn;
    }

    private static double GetDefaultMainWindowWidth()
    {
        var mainWindow = Application.Current.MainWindow;
        if (mainWindow != null)
        {
            return (double)mainWindow.FindResource("WindowBorder_Width");
        }
        else
        {
            // Behandle den Fall, wenn keine MainWindow-Instanz verfügbar ist
            throw new InvalidOperationException("MainWindow is not available.");
        }
    }

    /// <summary>
    /// Get Key Value from StaticResource
    /// </summary>
    /// <param name="resourcKey"></param>
    /// <returns></returns>
    private static double GetWidthFromResource(string resourceKey)
    {
        var mainWindow = Application.Current.MainWindow;
        if (mainWindow != null)
        {
            return (double)mainWindow.FindResource(resourceKey);
        }
        else
        {
            // Behandle den Fall, wenn keine MainWindow-Instanz verfügbar ist
            throw new InvalidOperationException("MainWindow is not available.");
        }
    }

    /// <summary>
    /// Rezise NavigationBar to wide
    /// </summary>
    private static void RestoreNavigationWidth()
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.NavColumn.Style = (Style)mainWindow.FindResource("NavColumnWidth_Style");
            SetMainWindowWidth(DefaultWindowWidth);
        }
    }

    /// <summary>
    ///  Rezise MainWindowWidth to new value
    /// </summary>
    /// <param name="width"></param>
    private static void SetMainWindowWidth(double width)
    {
        if (Application.Current.MainWindow is MainWindow mainWindow)
        {
            mainWindow.Width = width;
        }
    }
}