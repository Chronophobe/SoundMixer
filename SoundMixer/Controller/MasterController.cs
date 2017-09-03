using NAudio.CoreAudioApi;
using SoundMixer.Serial;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace SoundMixer.Controller
{
    class MasterController : IController
    {
        public bool SetVolume(float volume)
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia | Role.Console);
            device.AudioEndpointVolume.MasterVolumeLevelScalar = ((float)volume) / 100;
            return true;
        }
    }
}
