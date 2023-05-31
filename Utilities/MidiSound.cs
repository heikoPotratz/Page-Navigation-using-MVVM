using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MidiInput
{
    public class MidiSound : IDisposable
    {
        // How to programmatically set the system volume?
        // volume : https://stackoverflow.com/questions/13139181/how-to-programmatically-set-the-system-volume

        // To detect redundant calls
        private bool _disposedValue;

        private ushort _frequency;
        private byte _midiNumber;
        private MemoryStream _mStream;
        private string _noteName;
        private bool _noteOn;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        private System.Media.SoundPlayer _sPlayer = new System.Media.SoundPlayer();

        private ushort _velocity;

        public MidiSound()
        {
        }

        /// <summary>
        /// Constructor with MidiNumber and Velocity
        /// </summary>
        /// <param name="midiNumber"></param>
        /// <param name="velocity"></param>
        public MidiSound(byte midiNumber, ushort velocity = 100)
        {
            SetProperties(midiNumber, velocity);

            MidiSoundAction();
        }

        /// <summary>
        /// Constructor with NoteName and Velocity
        /// </summary>
        /// <param name="noteName"></param>
        /// <param name="velocity"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MidiSound(string noteName, ushort velocity = 100)
        {
            if (noteName == null || noteName == "")
            {
                throw new ArgumentNullException("noteName must not be null or empty");
            }

            SetProperties(noteName, velocity);

            MidiSoundAction();
        }

        public int MidiNumber
        {
            get => _midiNumber;
        }

        public string NoteName
        {
            get => _noteName;
        }

        public bool NoteOn
        {
            get => _noteOn;
        }

        public int Velocity => _velocity;

        public void Dispose()
        {
            // https://learn.microsoft.com/de-de/dotnet/standard/garbage-collection/implementing-dispose
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void NoteOff()
        {
            _noteOn = false;
        }

        /// <summary>
        /// SoundPlayer play a tone
        /// </summary>
        public void Play()
        {
            _sPlayer.Play();
        }

        public void Shift(int octaveSpan, int midiID, out string noteName, out byte midiNumber)
        {
            midiNumber = (byte)midiID;
            var minValueKey = 65;

            var maxValueKey = 4186;

            var newMidiValue = (octaveSpan * 12) + midiNumber;

            if (newMidiValue > maxValueKey)
                throw new ArgumentException("Wert ausserhalb der Midiskale!");

            noteName = CalcMidiValue.NoteName(newMidiValue);
            midiNumber = (byte)newMidiValue;
        }

        public void Stop()
        {
            _sPlayer.Stop();
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _safeHandle.Dispose();
                }

                _disposedValue = true;
            }
        }

        private static MemoryStream GetMemoryStream(ushort frequency, int msDuration, int volume = 16383)
        {
            var mStrm = new MemoryStream();
            var writer = new BinaryWriter(mStrm);

            #region the variables

            const double TAU = 2 * Math.PI;
            int formatChunkSize = 16;
            int headerSize = 8;
            short formatType = 1;
            short tracks = 1;
            int samplesPerSecond = 44100;
            short bitsPerSample = 16;
            short frameSize = (short)(tracks * ((bitsPerSample + 7) / 8));
            int bytesPerSecond = samplesPerSecond * frameSize;
            int waveSize = 4;
            int samples = (int)((decimal)samplesPerSecond * msDuration / 1000);
            int dataChunkSize = samples * frameSize;
            int fileSize = waveSize + headerSize + formatChunkSize + headerSize + dataChunkSize;

            #endregion the variables

            #region definition of the header

            // var encoding = new System.Text.UTF8Encoding();
            writer.Write(0x46464952); // = encoding.GetBytes("RIFF")
            writer.Write(fileSize);
            writer.Write(0x45564157); // = encoding.GetBytes("WAVE")
            writer.Write(0x20746D66); // = encoding.GetBytes("fmt ")
            writer.Write(formatChunkSize);
            writer.Write(formatType);
            writer.Write(tracks);
            writer.Write(samplesPerSecond);
            writer.Write(bytesPerSecond);
            writer.Write(frameSize);
            writer.Write(bitsPerSample);
            writer.Write(0x61746164); // = encoding.GetBytes("data")
            writer.Write(dataChunkSize);

            #endregion definition of the header

            #region writing the values

            var theta = frequency * TAU / samplesPerSecond;
            // 'volume' is UInt16 with range 0 thru Uint16.MaxValue ( = 65 535)
            // we need 'amp' to have the range of 0 thru Int16.MaxValue ( = 32 767)
            double amp = volume >> 2; // so we simply set amp = volume / 2

            for (var step = 0; step < samples; step++)
            {
                // fade out the volume
                var fadeout = FadeOutFactor(step, samples);
                var s = (short)(fadeout * amp * Math.Sin(theta * step));
                writer.Write(s);
            }

            // mStrm.Seek(0, SeekOrigin.Begin);

            #endregion writing the values

            return mStrm;

            #region Local function to  fade out the volume

            static double FadeOutFactor(int step, int samples)
            {
                return 1.0 - (double)step / samples;
            }

            static double Sigmoidfunction(int x)
            {
                return 1 / (1 + Math.Exp(-x));
            }

            #endregion Local function to  fade out the volume
        }

        private static int GetOctavenumber(string keyname)
        {
            var aString = keyname;// "ABCDE99F-J74-12-89A";

            // Select only those characters that are numbers
            var stringQuery =
              from ch in aString
              where char.IsDigit(ch)
              select ch;

            // Execute the query
            var newStringOfDigitsOnly = "";
            foreach (var c in stringQuery)
            {
                newStringOfDigitsOnly += c.ToString();
            }

            var result = int.Parse(newStringOfDigitsOnly);

            return result;
        }

        private void InitializeStream()
        {
            var volume = 16383 * _velocity / 127;
            _mStream = GetMemoryStream(_frequency, 1600, volume);

            _mStream.Seek(0, SeekOrigin.Begin);
        }

        private void MidiSoundAction()
        {
            if (_noteOn)
            {
                InitializeStream();
                _sPlayer = new System.Media.SoundPlayer(_mStream);
            }
            else
            {
                _sPlayer.Stop();
            }
        }

        private void SetProperties(int midiNumber, ushort velocity = 100)
        {
            _noteName = CalcMidiValue.NoteName(midiNumber, true);
            _midiNumber = (byte)midiNumber;
            _frequency = (ushort)CalcMidiValue.Frequency(midiNumber);
            _velocity = velocity;
            _noteOn = velocity != 0;
        }

        private void SetProperties(string noteName, ushort velocity = 100)
        {
            _noteName = noteName;
            _midiNumber = (byte)CalcMidiValue.MidiNumber(noteName);
            _frequency = (ushort)CalcMidiValue.Frequency(_midiNumber);
            _velocity = velocity;
            _noteOn = velocity != 0;
        }

        private string ShiftToOctave(string keyname, int newOctave)
        {
            int number = GetOctavenumber(keyname);
            string newKeyname = keyname.Replace(number.ToString(), newOctave.ToString());
            return newKeyname;
        }
    }
}