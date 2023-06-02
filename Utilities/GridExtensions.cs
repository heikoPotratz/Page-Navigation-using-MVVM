using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Page_Navigation_App.Utilities;

/// <summary>
/// Provides extension methods for the <see cref="Grid"/> class.
/// </summary>
public static class GridExtensions
{
    /// <summary>
    /// Adds new rows to the grid with the specified names.
    /// </summary>
    /// <param name="grid">The grid to add rows to.</param>
    /// <param name="rowNames">The names of the rows to add.</param>
    public static void AddRows(this Grid grid, List<string> rowNames)
    {
        foreach (var rowName in rowNames)
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Name = rowName;
            grid.RowDefinitions.Add(rowDefinition);
        }
    }

    /// <summary>
    /// Finds a child element with a specific name and retrieves the x:Name and RowIndex of its parent Row in the parent Grid.
    /// </summary>
    /// <param name="parent">The parent element (Grid) to search within.</param>
    /// <param name="childName">The name of the child element to find.</param>
    /// <returns>A tuple containing the x:Name and RowIndex of the parent Row if found; otherwise, null.</returns>
    public static (string Name, int? RowIndex) FindGridChildParentRowInfo(this Grid parent, string childName)
    {
        if (parent == null || string.IsNullOrEmpty(childName))
        {
            return (null, null);
        }

        foreach (UIElement child in parent.Children)
        {
            if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
            {
                int? rowIndex = Grid.GetRow(frameworkElement);
                if (rowIndex != null && parent.RowDefinitions.Count > rowIndex)
                {
                    var rowName = parent.RowDefinitions[rowIndex.Value].Name;
                    return (rowName, rowIndex);
                }
            }
        }

        return (null, null);
    }

    /// <summary>
    /// Finds a child element with a specific name and retrieves the x:Name of its parent Row in the parent Grid.
    /// </summary>
    /// <param name="parent">The parent element (Grid) to search within.</param>
    /// <param name="childName">The name of the child element to find.</param>
    /// <returns>The x:Name of the parent Row if found; otherwise, null.</returns>
    public static string FindGridChildParentRowName(this Grid parent, string childName)
    {
        if (parent == null || string.IsNullOrEmpty(childName))
        {
            return null;
        }

        foreach (UIElement child in parent.Children)
        {
            if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
            {
                if (Grid.GetRow(frameworkElement) is int rowIndex && parent.RowDefinitions.Count > rowIndex)
                {
                    return parent.RowDefinitions[rowIndex].Name;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Finds a child element with a specific name and retrieves its row index in the parent Grid.
    /// </summary>
    /// <param name="parent">The parent element (Grid) to search within.</param>
    /// <param name="childName">The name of the child element to find.</param>
    /// <returns>The row index of the child element if found; otherwise, -1.</returns>
    public static int FindGridChildRowIndex(this Grid parent, string childName)
    {
        if (parent == null || string.IsNullOrEmpty(childName))
        {
            return -1;
        }

        for (var i = 0; i < parent.RowDefinitions.Count; i++)
        {
            foreach (UIElement child in parent.Children)
            {
                if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                {
                    var rowIndex = Grid.GetRow(frameworkElement);
                    if (rowIndex == i)
                    {
                        return i;
                    }
                }
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the index of the last row in the grid.
    /// </summary>
    /// <param name="grid">The grid to get the last row index from.</param>
    /// <returns>The index of the last row.</returns>
    public static int GetLastRowIndex(this Grid grid)
    {
        return grid.RowDefinitions.Count - 1;
    }

    /// <summary>
    /// Gets the name of the specified <see cref="FrameworkElement"/>.
    /// </summary>
    /// <param name="element">The <see cref="FrameworkElement"/> to get the name from.</param>
    /// <returns>The name of the element.</returns>
    public static string GetName(this FrameworkElement element)
    {
        return element.Name;
    }

    /// <summary>
    /// Gets the index of the row with the specified name in the grid.
    /// </summary>
    /// <param name="grid">The grid to search for the row index.</param>
    /// <param name="rowName">The name of the row.</param>
    /// <returns>The index of the row, or -1 if no match is found.</returns>
    public static int GetRowIndex(this Grid grid, string rowName)
    {
        for (var i = 0; i < grid.RowDefinitions.Count; i++)
        {
            if (grid.RowDefinitions[i].Name == rowName)
            {
                return i;
            }
        }

        return -1;
    }

    /// <summary>
    /// Gets the name of the row at the specified index in the grid.
    /// </summary>
    /// <param name="grid">The grid to get the row name from.</param>
    /// <param name="rowIndex">The index of the row.</param>
    /// <returns>The name of the row, or an empty string if the index is invalid.</returns>
    public static string GetRowName(this Grid grid, int rowIndex)
    {
        if (rowIndex >= 0 && rowIndex < grid.RowDefinitions.Count)
        {
            return grid.RowDefinitions[rowIndex].Name;
        }

        return string.Empty;
    }

    /// <summary>
    /// Moves the specified child grid to the specified row in the grid.
    /// </summary>
    /// <param name="grid">The grid containing the child grid.</param>
    /// <param name="childGrid">The child grid to move.</param>
    /// <param name="rowName">The name of the row to move the child grid to.</param>
    /// <exception cref="ArgumentNullException">Thrown if either the <paramref name="grid"/> or <paramref name="childGrid"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="rowName"/> is not found in the grid.</exception>
    public static void Move(this Grid grid, Grid childGrid, string rowName)
    {
        if (grid == null)
        {
            throw new ArgumentNullException(nameof(grid));
        }

        if (childGrid == null)
        {
            throw new ArgumentNullException(nameof(childGrid));
        }

        var rowIndex = grid.GetRowIndex(rowName);
        if (rowIndex == -1)
        {
            throw new ArgumentException("The specified row name was not found in the grid.", nameof(rowName));
        }

        grid.Children.Remove(childGrid);
        Grid.SetRow(childGrid, rowIndex);
        grid.Children.Add(childGrid);
    }

    /// <summary>
    /// Moves the child grid with the specified name to the specified row in the grid.
    /// </summary>
    /// <param name="grid">The grid containing the child grid.</param>
    /// <param name="childName">The name of the child grid to move.</param>
    /// <param name="rowName">The name of the row to move the child grid to.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="grid"/> is null.</exception>
    /// <exception cref="ArgumentException">
    /// Thrown if the <paramref name="childName"/> is not found in the grid,
    /// or if the <paramref name="rowName"/> is not found in the grid.
    /// </exception>
    public static void Move(this Grid grid, string childName, string rowName)
    {
        if (grid == null)
        {
            throw new ArgumentNullException(nameof(grid));
        }

        UIElement childGrid = grid.Children.OfType<Grid>().FirstOrDefault(c => c.Name == childName);
        if (childGrid == null)
        {
            throw new ArgumentException("The specified child grid name was not found in the grid.", nameof(childName));
        }

        int rowIndex = grid.GetRowIndex(rowName);
        if (rowIndex == -1)
        {
            throw new ArgumentException("The specified row name was not found in the grid.", nameof(rowName));
        }

        grid.Children.Remove(childGrid);
        Grid.SetRow(childGrid, rowIndex);
        grid.Children.Add(childGrid);
    }

    /// <summary>
    /// Finds a child element of the specified type within the visual tree of the specified parent element.
    /// </summary>
    /// <typeparam name="T">The type of the child element to find.</typeparam>
    /// <param name="parent">The parent element to search within.</param>
    /// <param name="child">The name of the child element to find.</param>
    /// <returns>The found child element, or null if not found.</returns>
    private static T FindChild<T>(this DependencyObject parent, string child) where T : DependencyObject
    {
        if (parent == null || string.IsNullOrEmpty(child))
        {
            return null;
        }

        var childCount = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < childCount; i++)
        {
            var c = VisualTreeHelper.GetChild(parent, i);
            if (c is T typedChild && (c as FrameworkElement)?.Name == child)
            {
                return typedChild;
            }

            var foundChild = FindChild<T>(c, child);
            if (foundChild != null)
            {
                return foundChild;
            }
        }

        return null;
    }

    /*    /// <summary>
        /// Sets the fill color of the specified Path object contained within the child Grid using a SolidColorBrush.
        /// </summary>
        /// <param name="grid">The Grid object containing the child Path.</param>
        /// <param name="childGridName">The name of the child Grid containing the Path.</param>
        /// <param name="color">The color to set as the fill.</param>
        /// <exception cref="ArgumentNullException">Thrown if the <paramref name="grid"/> or <paramref name="childGridName"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the child Grid or Path is not found.</exception>
        public static void SetChildPathFillColor(this Grid grid, string childGridName, Color color)
        {
            if (grid == null)
            {
                throw new ArgumentNullException(nameof(grid));
            }

            if (childGridName == null)
            {
                throw new ArgumentNullException(nameof(childGridName));
            }

            // Find the child Grid
            var childGrid = grid.FindChild<Grid>(childGridName);
            if (childGrid == null)
            {
                throw new InvalidOperationException($"Child Grid '{childGridName}' not found.");
            }

            // Find the Path object within the child Grid
            var path = childGrid.FindChild<Path>("ReceivedKeyGeometry");
            if (path == null)
            {
                throw new InvalidOperationException("Path object not found within the child Grid.");
            }

            // Set the fill color using a SolidColorBrush
            var brush = new SolidColorBrush(color);
            path.Fill = brush;
        }*/
}