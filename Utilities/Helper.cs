using System;
using System.Collections.Generic;

namespace Page_Navigation_App.Utilities;

/// <summary>
/// Helper class for page navigation.
/// </summary>
public static class Helper
{
    private static readonly Random _random = new();

    /// <summary>
    /// Gets a random key from a list of default values.
    /// </summary>
    /// <returns>A random key.</returns>
    public static string GetRandomKey()
    {
        List<string> defaultValues = new List<string> { "A", "B", "C", "D", "E", "F", "G" };
        var index = _random.Next(defaultValues.Count);
        return defaultValues[index];
    }

    /// <summary>
    /// Gets a random key from an array of default values.
    /// </summary>
    /// <param name="defaultValues">An array of default values.</param>
    /// <returns>A random key.</returns>
    public static string GetRandomKey(string[] defaultValues)
    {
        var index = _random.Next(defaultValues.Length);
        return defaultValues[index];
    }
}