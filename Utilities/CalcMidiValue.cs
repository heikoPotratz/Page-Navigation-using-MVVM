using System;
using System.Collections.Generic;

namespace MidiInput
{
    public static class CalcMidiValue
    {
        private static readonly Dictionary<string, int> keyNumber = new Dictionary<string, int>()
        {
                {"c",0}, {"d",2}, {"e",4}, {"f",5}, {"g",7}, {"a",9}, {"b",11}, {"h",11}
        };

        /// <summary>
        /// Calculate the friequence on midinumber
        /// </summary>
        /// <param name="midiNumber"></param>
        /// <returns></returns>
        public static double Frequency(int midiNumber)
        {
            double a = 432; // Standardstimmung 440
            var factor = (midiNumber - 69) / 12.0;
            var freq = a * Math.Pow(2.0, factor);

            return freq;
        }

        /// <summary>
        /// Determine the midi number according to the Enhamonic confusion: ('#'/'b')
        /// </summary>
        /// <param name="noteName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int MidiNumber(string noteName)
        {
            if (noteName.Length < 2 || noteName.Length > 3)
            {
                var ArgumentExceptionMsg = $"Ungültiger Notenname: '{noteName}'";

                throw new ArgumentException(ArgumentExceptionMsg);
            }

            var addAccidentalsValue = 0;
            if (noteName.Contains("#"))
            {
                addAccidentalsValue = noteName.Contains("#") ? 1 : 0;
            }
            else if (noteName.Contains("b"))
            {
                addAccidentalsValue = noteName.Contains("b") ? -1 : 0;
            }

            var note = noteName[0].ToString().ToLower();
            var octave = OctaveNumber(noteName);
            var addNumber = keyNumber[note];
            var result = (12 * octave) + addNumber + 12;

            return result += addAccidentalsValue;
        }

        /// <summary>
        /// Get then the Nameof a Note by it's Midinumber dpending on sharp- or flatt-Scale
        /// </summary>
        /// <param name="midiNumber"></param>
        /// <param name="withSharp"></param>
        /// <returns></returns>
        public static string NoteName(int midiNumber, bool withSharp = true)
        {
            string[] flatMode = { "C", "Db", "D", "Eb", "E", "F", "Gb", "G", "Ab", "A", "Bb", "B" };
            string[] sharpMode = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
            var noteIndex = midiNumber % 12;
            string noteName;
            if (withSharp)
            {
                noteName = sharpMode[noteIndex];
            }
            else
            {
                noteName = flatMode[noteIndex];
            }
            var octave = (midiNumber / 12) - 1;
            return noteName + octave.ToString();
        }

        /// <summary>
        /// Get the octavenumber
        /// </summary>
        /// <param name="noteName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int OctaveNumber(string noteName)
        {
            if (noteName.Length >= 2 && char.IsDigit(noteName[^1]))
            {
                var octave = int.Parse(noteName[^1..]);

                return octave;
            }
            else
            {
                throw new ArgumentException($"Ungültiger Notenname: '{noteName}'");
            }
        }

        public static bool Test()
        {
            var testNoteName = TestNoteName(61, "Db4", false);

            var testMidiNumber = TestMidiNumber("Db4", 61);

            return testNoteName && testMidiNumber;
        }

        public static bool TestMidiNumber(string noteName, int result)
        {
            return result == MidiNumber(noteName);
        }

        public static bool TestNoteName(int midiNumber, string result, bool withSharps = true)
        {
            return result == NoteName(midiNumber, withSharps);
        }
    }
}