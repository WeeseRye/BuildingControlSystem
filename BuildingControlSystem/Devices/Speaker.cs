using System;
using System.Collections.Generic;
using System.Text;

namespace BuildingControlSystem.Devices
{
    public class Speaker : Device
    {
        public enum SoundType
        {
            None,
            Music,
            Alarm
        }

        public override DeviceType Type => DeviceType.Speaker;

        private SoundType _sound;
        public SoundType Sound
        {
            get => _sound;
            set
            {
                _sound = value;
                Console.WriteLine(GetCurrentState());
            }
        }

        private float _volume;
        public float Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                Console.WriteLine(GetCurrentState());
            }
        }

        public Speaker(string name) : base(name)
        {
            _sound = SoundType.None;
            _volume = 0.5f; // výchozí hlasitost
        }

        public override string GetCurrentState()
        {
            return $"Sound={Sound}, Volume={Volume}";
        }
    }
}
