using System;
using System.Windows.Media.Animation;
using Page_Navigation_App.Model;
using Page_Navigation_App.Utilities;

namespace Page_Navigation_App.ViewModel;

internal class TrebleClefVM : ViewModelBase
{
    private readonly PageModel _pageModel;

    private string[] keyValues = { "C", "D", "E", "F", "G" };

    public TrebleClefVM()
    {
        _pageModel = new PageModel();
        ExpectedKeyName = "C4";
    }

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
        }
    }

    public void SetNextExpectedKey()
    {
        ExpectedKeyName = Helper.GetRandomKey(keyValues) + "4";
    }

    public void SetNextRecevedKey()
    {
        RecevedKeyName = Helper.GetRandomKey(keyValues) + "4";
    }

    public void SetNextRecevedKey(string keyname)
    {
        RecevedKeyName = keyname + "4";
    }

    /// <summary>
    /// Update Text Property MessageText
    /// </summary>
    /// <param name="msg"></param>
    public void UpdateMessageText(string msg)
    {
        _pageModel.MessageText = msg;
    }

    internal void SetNextExpectedKey(string v) => RecevedKeyName = v + "4";
}