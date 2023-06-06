/// <Summary>
/// Author : R. Arun Mutharasu
/// Created :17-01-2022
/// YouTube Channel : C# Design Pro
/// </Summary>

using System.ComponentModel;
using System.Windows.Input;

namespace WPFControlVisibilityApp.ViewModel;

public class AppViewModel : INotifyPropertyChanged
{
    private ICommand _hidePanelCommand;
    private bool _isPanelVisible;
    private ICommand _showPanelCommand;

    public AppViewModel()
    {
        // Set Default Panel Visibility //
        IsPanelVisible = false;
    }

    public event PropertyChangedEventHandler PropertyChanged;

    // Panel Visibility Property //
    public bool IsPanelVisible
    {
        get => _isPanelVisible;
        set
        {
            _isPanelVisible = value;
            OnPropertyChanged("IsPanelVisible");
        }
    }

    // Show Panel Method //
    public void ShowPanel()
    {
        IsPanelVisible = true;
    }

    private void OnPropertyChanged(string propName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}