using System;
using System.Collections.Generic;
using NAudio.Midi;
using System.ComponentModel;

namespace Page_Navigation_App.Utilities
{
    /// <summary>
    /// Watches MIDI input devices for connection and disconnection events.
    /// </summary>
    public class MidiWatcher : INotifyPropertyChanged, IMidiWatcher
    {
        private List<MidiIn> connectedDevices;
        private Dictionary<string, int> deviceIndexMap;
        private List<string> midiInDeviceList;

        /// <summary>
        /// Initializes a new instance of the MidiWatcher class.
        /// </summary>
        public MidiWatcher()
        {
            midiInDeviceList = new List<string>();
            connectedDevices = new List<MidiIn>();
            deviceIndexMap = new Dictionary<string, int>();
            RefreshDeviceList();
        }

        public event EventHandler<string> DeviceConnected;

        public event EventHandler<string> DeviceDisconnected;

        /// <summary>
        /// Gets the list of connected MIDI input devices.
        /// </summary>
        public List<MidiIn> ConnectedDevices
        {
            get
            {
                return connectedDevices;
            }
        }

        /// <summary>
        /// Gets the dictionary mapping device names to their index values.
        /// </summary>
        public Dictionary<string, int> DeviceIndexMap
        {
            get
            {
                return deviceIndexMap;
            }
        }

        /// <summary>
        /// Gets the list of available MIDI input devices.
        /// </summary>
        public List<string> MidiInDeviceList
        {
            get
            {
                return midiInDeviceList;
            }
            private set
            {
                midiInDeviceList = value;
                OnPropertyChanged(nameof(MidiInDeviceList));
            }
        }

        /// <summary>
        /// Refreshes the list of available MIDI input devices.
        /// </summary>
        public void RefreshDeviceList()
        {
            var previousDeviceList = new List<string>(midiInDeviceList);
            midiInDeviceList.Clear();
            connectedDevices.ForEach(midiIn => midiIn.Dispose());
            connectedDevices.Clear();
            deviceIndexMap.Clear();

            for (int device = 0; device < MidiIn.NumberOfDevices; device++)
            {
                MidiIn midiIn = new MidiIn(device);
                midiIn.MessageReceived += OnMessageReceived;
                midiIn.ErrorReceived += OnErrorReceived;
                midiIn.Start();

                connectedDevices.Add(midiIn);
                string deviceName = MidiIn.DeviceInfo(device).ProductName;
                deviceIndexMap[deviceName] = device;
                midiInDeviceList.Add(deviceName);
            }

            CheckDeviceChanges(previousDeviceList);
        }

        private void CheckDeviceChanges(List<string> previousDeviceList)
        {
            foreach (var deviceName in previousDeviceList)
            {
                if (!midiInDeviceList.Contains(deviceName))
                {
                    DeviceDisconnected?.Invoke(this, deviceName);
                    ShowStatusMessage($"Device disconnected: {deviceName}");
                }
            }

            foreach (var deviceName in midiInDeviceList)
            {
                if (!previousDeviceList.Contains(deviceName))
                {
                    DeviceConnected?.Invoke(this, deviceName);
                    ShowStatusMessage($"Device connected: {deviceName}");
                }
            }
        }

        private void OnErrorReceived(object sender, MidiInMessageEventArgs e) => throw new NotImplementedException();

        private void OnMessageReceived(object sender, MidiInMessageEventArgs e)
        {
            try
            {
                if (e.MidiEvent is NoteEvent noteEvent)
                {
                    var noteNumber = noteEvent.NoteNumber;

                    var velocity = noteEvent.Velocity;

                    var noteName = noteEvent.NoteName;
                    // using MidiSound m = new((byte)noteNumber, (byte)velocity);
                }
            }
            catch (InvalidCastException ex)
            {
                throw;
            }
        }

        private void ShowStatusMessage(string message)
        {
            // Zeigen Sie die Statusmeldung hier an (z. B. MessageBox oder Aktualisierung der Benutzeroberfläche)
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Implementation
    }
}