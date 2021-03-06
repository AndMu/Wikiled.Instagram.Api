﻿using System;
using Wikiled.Instagram.Api.Classes.Models.Direct;
using Wikiled.Instagram.Api.Classes.ResponseWrappers.Direct;

namespace Wikiled.Instagram.Api.Converters.Directs
{
    internal class InstaAudioConverter : IObjectConverter<InstaAudio, InstaAudioResponse>
    {
        public InstaAudioResponse SourceObject { get; set; }

        public InstaAudio Convert()
        {
            if (SourceObject == null)
            {
                throw new ArgumentNullException("Source object");
            }

            var audio = new InstaAudio
            {
                AudioSource = SourceObject.AudioSource,
                Duration = SourceObject.Duration,
                WaveformData = SourceObject.WaveformData,
                WaveformSamplingFrequencyHz = SourceObject.WaveformSamplingFrequencyHz
            };

            return audio;
        }
    }
}