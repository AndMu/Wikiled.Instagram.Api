using System;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaAudio
    {
        private double _duration;

        public string AudioSource { get; set; }

        public double Duration
        {
            get => _duration;
            set
            {
                _duration = value;
                DurationTs = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan DurationTs { get; set; }

        public float[] WaveformData { get; set; }

        public int WaveformSamplingFrequencyHz { get; set; }
    }
}
