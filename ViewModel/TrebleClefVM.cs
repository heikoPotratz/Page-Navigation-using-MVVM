using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using Page_Navigation_App.Model;
using Page_Navigation_App.Utiel;
using Page_Navigation_App.View;

namespace Page_Navigation_App.ViewModel;

internal class TrebleClefVM : ViewModelBase
{
    private static readonly string octaveNumber = "4";
    private readonly PageModel _pageModel;
    private Grid _theClefGrid;
    private string[] keyValues = { "C", "D", "E", "F", "G" };

    public TrebleClefVM()
    {
        _pageModel = new PageModel();
        SetRandomExpectedKey();

        // _theClefGrid initialisieren
        _theClefGrid = new Grid();

        // Erstelle eine Instanz von TheClef
    }

    public event Action<string> RecevedKeyNameChanged;

    public string ExpectedKeyName
    {
        get => _pageModel.ExpectedKeyName;
        set
        {
            _pageModel.ExpectedKeyName = value; OnPropertyChanged();
        }
    }

    public string MessageText
    {
        get => _pageModel.MessageText;
        set
        {
            _pageModel.MessageText = value; OnPropertyChanged();
        }
    }

    public string RecevedKeyName
    {
        get => _pageModel.RecevedKeyName;
        set
        {
            _pageModel.RecevedKeyName = value; OnPropertyChanged();

            // Event auslösen, um die Änderung der RecevedKeyName-Eigenschaft zu signalisieren
            // wird ausgefürt!
            RecevedKeyNameChanged?.Invoke(value);
        }
    }

    public Grid TheClefGrid
    {
        get => _theClefGrid;
        set
        {
            _theClefGrid = value;
            OnPropertyChanged();
        }
    }

    public void SetNextExpectedKey(string v) => ExpectedKeyName = v + octaveNumber;

    public void SetNextRecevedKey(string keyname)
    {
        RecevedKeyName = keyname;

        // Debugging only
        UpdateMessageText(keyname, true);
    }

    /// <summary>
    /// Get Random Keyname
    /// </summary>
    public void SetRandomExpectedKey()
    {
        ExpectedKeyName = Helper.GetRandomKey(keyValues) + octaveNumber;
    }

    // for Debugging only
    public void SetRandomRecevedKey()
    {
/*        RecevedKeyName = Helper.GetRandomKey(keyValues) + octaveNumber;
        ExpectedKeyName = GridOfExpectedKey//.FindGridChildParentRowName()

        _trebleClefVM.RecevedKeyName = "";
        // Debugging only
        UpdateMessageText(RecevedKeyName, true);*/
    }

    /// <summary>
    /// Update Text Property MessageText
    /// </summary>
    /// <param name="msg"></param>
    public void UpdateMessageText(string msg, bool debuggingOnly, [CallerMemberName] string callerMethodName = "")
    {
        msg += debuggingOnly ? $" (Debugging only)  Called by: {callerMethodName}" : "";
        _pageModel.MessageText = msg;
    }
}