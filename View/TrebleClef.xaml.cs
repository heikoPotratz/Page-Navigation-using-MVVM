using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MidiInput;
using Page_Navigation_App.Utilities;
using Page_Navigation_App.ViewModel;
using static Page_Navigation_App.Utilities.CustomConverter;

namespace Page_Navigation_App.View;

/// <summary>
/// Interaktionslogik für TrebleClef.xaml
/// </summary>
public partial class TrebleClef : UserControl
{
    private readonly Grid _theClefGrid;
    private bool _IsKeyMatched = false;
    private bool _keyDown = false;
    private bool _lastKeyIsDown = false;
    private PlayingHand _playingHand = PlayingHand.Right;
    private TrebleClefVM _trebleClefVM = new();
    private bool _useKeyboardAsDefault = true;

    public TrebleClef()
    {
        InitializeComponent();

        UserControlViewModelType = typeof(TrebleClefVM);

        KeyDown += Window_KeyDown;
        KeyUp += Window_KeyUp;

        // Den Fokus auf das UserControl setzen
        Keyboard.Focus(this);

        //In der View abonnierst du das Event in der ViewModel-Klasse und führst den Move-Befehl aus:
        SubscribeToViewModelEvents();

        DataContext = new TrebleClefVM();

        //
        if (DataContext is TrebleClefVM _trebleClefVM)
        {
            _trebleClefVM.SetRandomExpectedKey();

            ShowNextExpectedKey(_trebleClefVM.ExpectedKeyName);
        }
        else
        {
            throw new InvalidOperationException("DataContext is not of type TrebleClefVM.");
        }

        // Erstellen und Zuweisen eines neuen Grid-Objekts
        _theClefGrid = new();

        // Weitere Initialisierungen oder Konfigurationen des Grid-Objekts

        // DataContext auf das ViewModel setzen
        _theClefGrid.DataContext = new TrebleClefVM();

        //TheClef.Move(GridOfReceivedKey, trebleClefVM.RecevedKeyName);
    }

    public Type UserControlViewModelType
    {
        get; set;
    }

    private void btnNextExpectedKey_Click(object sender, RoutedEventArgs e)
    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            trebleClefVM.SetRandomExpectedKey();
        }
    }

    private void btnNextRecevedKey_Click(object sender, RoutedEventArgs e)
    {
        _trebleClefVM.SetRandomRecevedKey();
        // Dispay result
        _trebleClefVM.MessageText = GetRowIndex();

        ShowHideRecevedKey(true);

        TheClef.Move(GridOfReceivedKey, _trebleClefVM.RecevedKeyName);

        _IsKeyMatched = (_trebleClefVM.RecevedKeyName == _trebleClefVM.ExpectedKeyName);

        if (_IsKeyMatched)
        {
            PlaySound(_trebleClefVM.RecevedKeyName);
        }
    }

    /// <summary>
    /// Convert Windowds.Key to spezific Character for playing in the TrebleClaf
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    private string ConvertKey(KeyEventArgs e)
    {
        return ((ICustomConverter)new CustomConverter()).ConvertKey(e, _playingHand);
    }

    private void DoActionLogic()
    {
        // Save result of incomming key
        _IsKeyMatched = _trebleClefVM.ExpectedKeyName == _trebleClefVM.RecevedKeyName; // [x]

        // Display the Expected key in the clef
        DoShowExpectedNoteInTheClef(_trebleClefVM.ExpectedKeyName, false); // [x]

        if (_keyDown) // [x]
        {   // counter logic only when KeyDown / NoteOn
            //_staticCounter.Add(_IsKeyMatched);

            // Playing the note
            if (_IsKeyMatched)
            {
                PlaySound(_trebleClefVM.ExpectedKeyName);

                // Hide RecevedKey
                ShowHideRecevedKey(false);

                // Show next Expected random Note
                _trebleClefVM.SetRandomExpectedKey();
                DoShowExpectedNoteInTheClef(_trebleClefVM.ExpectedKeyName, false);
            }
            else
            {   // or show only the note as false in red color
                DoShowReceivedNoteInTheClef(false); // [x]
            }
        }
        else
        {   // Hide RecevedKey
            ShowHideRecevedKey(false);
        }

        // Display value of counter
        //lblCounter.Content = $"{_staticCounter.SuccessRate * 100} %";

        // Finaly dispose the object
        //_midiSound.Dispose();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="keyname"></param>
    /// <param name="isbool"></param>
    private void DoShowExpectedNoteInTheClef(string keyname, bool isbool = false)
    {
        ShowNextExpectedKey(Helper.GetRandomKey());
    }

    private void DoShowReceivedNoteInTheClef(bool ismatched)
    {
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

    private void HandleRecevedKeyNameChanged(string receivedKeyName)
    {
        // Führe hier den Move-Befehl aus, um TheClef zu bewegen
        // wird ausgeführt
        TheClef.Move(GridOfReceivedKey, receivedKeyName);
    }

    private void OnKeyEventReceived(KeyEventArgs e, bool isKeyDown)
    {
        // _PlayingHand is set in MainWindow()

        // Set a defaut Octave number avter converted for forther calculation
        var keyname = ConvertKey(e) + "4";
        // var keyname = CustomConverter.ConvertKey(e, _playingHand);// ToNoteName(e, _PlayingHand);
        if (keyname.Length == 1)
        {
            return;
        }

        if (isKeyDown)
        {
            // Event in der ViewModel auslösen und den keyname-Wert übergeben

            _trebleClefVM = new();

            _trebleClefVM.RecevedKeyName = keyname;
            ShowHideRecevedKey(true);
            PlaySound(keyname);
        }
        else
        {
            ShowHideRecevedKey(false);
            // using MidiSound m = new(keyname, 0);
        }
    }

    private void PlaySound(string keyname)
    {
        ushort _DefaultVelocity = 45;
        using MidiSound m = new(keyname, _DefaultVelocity);
        m.Play();
    }

    private void SetDefaultValues()
    {
    }

    private void SetVisibilityOfReceivedKey(bool isvisible)
    {
    }

    // ShowHideRecevedKey(GridOfReceivedKey, true);
    private void ShowHideRecevedKey(bool show)
    {
        if (show)
        {
            GridOfReceivedKey.Visibility = Visibility.Visible;
        }
        else
        {
            GridOfReceivedKey.Visibility = Visibility.Hidden;
        }
    }

    private void ShowNextExpectedKey(string keyname)

    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            // trebleClefVM.SetNextRecevedKey(keyname);
            // Dispay result
            trebleClefVM.MessageText = GetRowIndex();

            TheClef.Move(GridOfExpectedKey, keyname);
        }
    }

    private void ShowNextExpectedKey()
    {
        ShowNextExpectedKey(Helper.GetRandomKey());
    }

    private void SubscribeToViewModelEvents()
    {   // wird ausgeführt!
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            trebleClefVM.RecevedKeyNameChanged += HandleRecevedKeyNameChanged;
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (_useKeyboardAsDefault)
        {
            if (_lastKeyIsDown == true)
            {
                return;
            }
            _lastKeyIsDown = true;
            OnKeyEventReceived(e, true);
        }
    }

    private void Window_KeyUp(object sender, KeyEventArgs e)
    {
        if (_useKeyboardAsDefault)
        {
            _lastKeyIsDown = false;
            OnKeyEventReceived(e, false);
        }
    }

    #region FocusControl

    public static readonly DependencyProperty IsUserControlFocusedProperty = DependencyProperty.Register(
     nameof(IsUserControlFocused), typeof(bool), typeof(TrebleClef), new PropertyMetadata(false));

    // _mytype
    public bool IsUserControlFocused
    {
        get
        {
            return (bool)GetValue(IsUserControlFocusedProperty);
        }
        set
        {
            SetValue(IsUserControlFocusedProperty, value);
        }
    }

    #endregion FocusControl
}