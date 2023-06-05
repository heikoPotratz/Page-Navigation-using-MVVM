using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MidiInput;
using Page_Navigation_App.Utiel;
using Page_Navigation_App.ViewModel;

using static Page_Navigation_App.Utiel.CustomConverter;

namespace Page_Navigation_App.View;

/// <summary>
/// Interaktionslogik für TrebleClef.xaml
/// </summary>
public partial class TrebleClef : UserControl
{
    private static string _lastExpectedKey = string.Empty;
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

        // Erstellen und Zuweisen eines neuen Grid-Objekts
        _theClefGrid = new()
        {
            // DataContext auf das ViewModel setzen
            DataContext = new TrebleClefVM()
        };

        SetDefaultValues();

        //Display a random key as expectedKey
        ShowNewExpectedKey();
    }

    public Type UserControlViewModelType
    {
        get; set;
    }

    private static void PlaySound(string keyname)
    {
        ushort _DefaultVelocity = 45;
        using MidiSound m = new(keyname, _DefaultVelocity);
        m.Play();
    }

    /// <summary>
    /// Returns a Notename Character
    /// </summary>
    /// <param name="e"></param>
    /// <param name="playingHand"></param>
    /// <returns></returns>
    private static string ToNoteName(KeyEventArgs e, CustomConverter.PlayingHand playingHand = CustomConverter.PlayingHand.Right)
    {
        var converTo = new CustomConverter();
        return ((ICustomConverter)converTo).ConvertKey(e, playingHand);
    }

    private void ActionLogic(string keyname)
    {
        // Event in der ViewModel auslösen und den keyname-Wert übergeben

        _trebleClefVM = new();

        _trebleClefVM.RecevedKeyName = keyname;

        ShowRecevedKeyOnIsMatchFaile(keyname);

        if (_IsKeyMatched)
        {
            PlaySound(keyname);

            ShowHideRecevedKey(false);

            // new Ecpected key for display
            ShowNewExpectedKey();
        }
        else
        {
            ShowHideRecevedKey(true);
        }
    }

    private void btnNextRecevedKey_Click(object sender, RoutedEventArgs e)
    {
        var keyname = Helper.GetRandomKey() + "4";
        ActionLogic(keyname);
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

    /// <summary>
    /// Just fordebuging info
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

    private void HandleRecevedKeyNameChanged(string receivedKeyName)
    {
        // Führe hier den Move-Befehl aus, um TheClef zu bewegen
        // wird ausgeführt
        TheClef.Move(GridOfReceivedKey, receivedKeyName);
    }

    /// <summary>
    /// I will need this for shure later on ....
    /// </summary>
    /// <param name="rowIndex"></param>
    /// <returns></returns>
    private bool IsOutOfRange(int rowIndex)
    {
        return false;
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
            ActionLogic(keyname);
        }
        else
        {
            // On keyUP hide Key
            ShowHideRecevedKey(false);
        }
    }

    /// <summary>
    /// SubcribeWindow_KeyEvents();
    /// SubscribeToViewModelEvents();
    /// Keyboard.Focus(this);
    /// DataContext = new TrebleClefVM();
    /// </summary>
    private void SetDefaultValues()
    {
        UserControlViewModelType = typeof(TrebleClefVM);

        SubcribeWindow_KeyEvents();

        //In der View abonnierst du das Event in der ViewModel-Klasse und führst den Move-Befehl aus:
        SubscribeToViewModelEvents();

        // Den Fokus auf das UserControl setzen
        Keyboard.Focus(this);

        DataContext = new TrebleClefVM();

        // on Start Hide the Receved Key
        ShowHideRecevedKey(false);
    }

    private void ShowExpectedKey(string keyname)

    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            // trebleClefVM.SetNextRecevedKey(keyname);
            // Dispay result
            var i = TheClef.GetRowIndex(keyname);
            trebleClefVM.ExpectedKeyName = keyname;
            trebleClefVM.MessageText = $"GridOfExpectedKey - GetRowIndex:  {i} of: {keyname}";

            TheClef.Move(GridOfExpectedKey, keyname);
        }
    }

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

    /// <summary>
    /// Show key in TrebleClef when _IsKeyMatched value is false
    /// </summary>
    /// <param name="keyname"></param>

    private void ShowNewExpectedKey()
    {
        var nextKey = Helper.GetRandomKey();

        if (nextKey != _lastExpectedKey)
        {
            ShowExpectedKey(nextKey + "4");

            // save last key
            _lastExpectedKey = nextKey + "4";
        }
        else
        {   // try again
            ShowNewExpectedKey();
        }
    }

    private void ShowRecevedKeyOnIsMatchFaile(string keyname)
    {
        if (DataContext is TrebleClefVM trebleClefVM)
        {
            // trebleClefVM.SetNextRecevedKey(keyname);
            // Dispay result
            var i = TheClef.GetRowIndex(keyname);
            trebleClefVM.RecevedKeyName = keyname;

            trebleClefVM.MessageText = $"GridOfReceivedKey - GetRowIndex:  {i} of: {keyname}";

            _IsKeyMatched = (trebleClefVM.ExpectedKeyName == trebleClefVM.RecevedKeyName);

            if (!_IsKeyMatched)
            {
                TheClef.Move(GridOfReceivedKey, keyname);
            }
        }
    }

    /// <summary>
    ///  Adding KeyDow and KeyUp
    /// </summary>
    private void SubcribeWindow_KeyEvents()
    {
        KeyDown += Window_KeyDown;
        KeyUp += Window_KeyUp;
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