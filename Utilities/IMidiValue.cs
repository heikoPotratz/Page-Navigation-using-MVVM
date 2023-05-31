namespace MidiInput
{
    public interface IMidiValue
    {
        double Frequency
        {
            get;
        }

        int MidiNumber
        {
            get;
        }

        string NoteName
        {
            get;
        }

        bool NoteOn
        {
            get;
        }

        int Velocity
        {
            get;
        }

        public void Dispose();
    }
}