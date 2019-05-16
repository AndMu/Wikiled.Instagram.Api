using Newtonsoft.Json;

namespace Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct
{
    public class InstaAudioResponse
    {
        [JsonProperty("audio_src")] public string AudioSource { get; set; }

        [JsonProperty("duration")] public double Duration { get; set; }

        [JsonProperty("waveform_data")] public float[] WaveformData { get; set; }

        [JsonProperty("waveform_sampling_frequency_hz")]
        public int WaveformSamplingFrequencyHz { get; set; }
    }
}
