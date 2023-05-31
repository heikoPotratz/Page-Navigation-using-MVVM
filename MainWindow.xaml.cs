﻿using System.Windows;
using System.Windows.Input;
using MidiInput;
using Page_Navigation_App.Utilities;
using Page_Navigation_App.View;
using Page_Navigation_App.ViewModel;
using static Page_Navigation_App.Utilities.KeyEventArgsConverter;

namespace Page_Navigation_App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static PlayingHand _PlayingHand = PlayingHand.Right;
        private MidiWatcher _midiWatcher;

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;

            // Registrieren der Eventhandler-Methoden für die KeyDown- und KeyUp-Events der MainWindow

            // Ereignisbindung
            //this.KeyDown += Window_KeyDown;

            KeyDown += /*KeyboardEventManager.*/Window_KeyDown;
            KeyUp += /*KeyboardEventManager.*/Window_KeyUp;

            // Registriere den Eventhandler für das KeyEventReceived-Event

            KeyboardEventManager.KeyEventReceived += KeyboardEventManager_KeyEventReceived;

            // Set NavigationBar to narrow
            WindowSizing.ShrinkNavigationWidth();
        }

        //

        #region statusmeldung

        private static readonly bool _useKeyboardAsDefault = true;

        private static bool _lastKeyIsDown = false;

        private static string PlayingHandToString()
        {
            var msg = "PlayingHand is currently set to the ";

            if (_PlayingHand == PlayingHand.Right)
            {
                return msg + "right!";
            }
            else
            {
                return msg + "left!";
            }
        }

        private static void SwapPlayingHand()
        {
            if (_PlayingHand == PlayingHand.Right)
            {
                _PlayingHand = PlayingHand.Left;
            }
            else
            {
                _PlayingHand = PlayingHand.Right;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _midiWatcher = new MidiWatcher();
            _midiWatcher.DeviceConnected += MidiWatcher_DeviceConnected;
            _midiWatcher.DeviceDisconnected += MidiWatcher_DeviceDisconnected;
            _midiWatcher.RefreshDeviceList();

            if (_midiWatcher.MidiInDeviceList.Count == 0)
            {
                ShowStatusMessage("No MIDI devices found.");
            }
            else
            {
                var msg = $"MIDI devices '{_midiWatcher.ConnectedDevices.Count}' found.";
                ShowStatusMessage(msg);
            }
        }

        private void MidiWatcher_DeviceConnected(object sender, string deviceName)
        {
            ShowStatusMessage($"MIDI device '{deviceName}' found.");
        }

        private void MidiWatcher_DeviceDisconnected(object sender, string deviceName)
        {
            // Code, um die Gerätetrennung zu verarbeiten
        }

        private void OnKeyEventReceived(KeyEventArgs e, bool isKeyDown)
        {
            // _PlayingHand is set in MainWindow()
            var keyname = ToNoteName(e, _PlayingHand);
            if (keyname == string.Empty)
            {
                return;
            }

            // Extrahieren Sie das DataContext aus dem XML-Objekt
            var pageDataContext = Pages.DataContext;

            // Erstellen Sie eine Instanz der TrebleClefVM-Klasse
            var trebleClefVM = new TrebleClefVM();

            // Rufen Sie die Methode im PageModel auf
            trebleClefVM.SetNextRecevedKey(keyname);

            // Rufen Sie die Methode im TrebleClef-UserControl auf
            /*            if (pageDataContext is TrebleClef trebleClef)
                        {
                            if (trebleClef.DataContext is TrebleClefVM trebleClefVM)
                            {
                                trebleClefVM.SetNextExpectedKey("D");
                            }
                        }*/

            if (isKeyDown)
            {
                ushort _DefaultVelocity = 120;
                using MidiSound m = new(keyname + "4", _DefaultVelocity);
            }
            else
            {
                using MidiSound m = new(keyname + "4", 0);
            }
        }

        private void ShowStatusMessage(string message)
        {
            // Zeigen Sie die Statusmeldung hier an (z. B. MessageBox oder Aktualisierung der Benutzeroberfläche)
            MessageBox.Show(message);
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

        #endregion statusmeldung

        //
        /// <summary>
        /// Returns a Notename Character0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string ToNoteName(KeyEventArgs e, PlayingHand playingHand = PlayingHand.Right)
        {
            return KeyEventArgsConverter.ConvertKey(e, playingHand);
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Flip Styles  (Strech or srink) Navigation Column
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HamburgerMenu_Click(object sender, RoutedEventArgs e)
        {
            WindowSizing.ToggleNavigationWidth();
        }

        private void KeyboardEventManager_KeyEventReceived(object sender, KeyEventArgs e)
        {
            // Handle den KeyEventReceived-Event hier
            // ...
        }
    }
}