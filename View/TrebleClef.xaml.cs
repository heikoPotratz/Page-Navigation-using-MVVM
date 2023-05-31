using System.Windows;
using System.Windows.Controls;
using Page_Navigation_App.Utilities;
using Page_Navigation_App.ViewModel;

namespace Page_Navigation_App.View;

/// <summary>
/// Interaktionslogik für TrebleClef.xaml
/// </summary>
public partial class TrebleClef : UserControl
{
    public TrebleClef()
    {
        InitializeComponent();
    }

    public void SetNextExpectedKey(string keyname)

    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            trebleClefVM.SetNextRecevedKey(keyname);
            // Dispay result
            trebleClefVM.MessageText = GetRowIndex();

            TheClef.Move(GridOfReceivedKey, trebleClefVM.RecevedKeyName);
        }
    }

    private void btnNextExpectedKey_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            trebleClefVM.SetNextExpectedKey();
        }
    }

    private void btnNextRecevedKey_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            trebleClefVM.SetNextRecevedKey();
            // Dispay result
            trebleClefVM.MessageText = GetRowIndex();

            TheClef.Move(GridOfReceivedKey, trebleClefVM.RecevedKeyName);
        }
    }

    /// <summary>
    /// Just forvdebuging info
    /// </summary>
    /// <returns></returns>
    private string GetMsg()
    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            string expMsg;
            if (!string.IsNullOrEmpty(trebleClefVM.ExpectedKeyName))
            {
                expMsg = $"ExpectedKeyName:  '{trebleClefVM.ExpectedKeyName}' ";
            }
            else
            {
                expMsg = $"ExpectedKeyName:  'IsNullOrEmpty' ";
            }

            string recMsg;
            if (!string.IsNullOrEmpty(trebleClefVM.RecevedKeyName))
            {
                recMsg = $"RecevedKeyName:  '{trebleClefVM.RecevedKeyName}' ";
            }
            else
            {
                recMsg = $"RecevedKeyName:  'IsNullOrEmpty' ";
            }

            return $"{recMsg} ; {expMsg}; Success = {trebleClefVM.RecevedKeyName == trebleClefVM.ExpectedKeyName}";
        }

        return "";
    }

    private string GetRowIndex()
    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            var index = TheClef.GetRowIndex(trebleClefVM.RecevedKeyName);
            return $" GetRowIndex() {index}";
        }
        return "GetRowIndex()";
    }
}