using System;

namespace Wikiled.Instagram.Api.Classes.Models.Direct
{
    public class InstaAudio
    {
        private double duration;

        public string AudioSource { get; set; }

        public double Duration
        {
            get => duration;
            set
            {
                duration = value;
                DurationTs = TimeSpan.FromMilliseconds(value);
            }
        }

        public TimeSpan DurationTs { get; set; }

        public float[] WaveformData { get; set; }

        public int WaveformSamplingFrequencyHz { get; set; }
    }
}