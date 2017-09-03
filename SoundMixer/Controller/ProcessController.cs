using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;

namespace SoundMixer.Controller
{
    class ProcessController : IController
    {
        public String Name { get; private set; }
        public AudioSessionControl Session { get; private set; }

        public ProcessController(String name)
        {
            this.Name = name;
            this.Session = this.getSession();
        }

        private AudioSessionControl getSession()
        {
            var processes = Process.GetProcessesByName(this.Name);
            var pids = processes.Select(x => x.Id);

            var deviceEnumerator = new MMDeviceEnumerator();
            var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia | Role.Console);
            var sessions = device.AudioSessionManager.Sessions;
            for(int i = 0; i < sessions.Count; i++)
            {
                var session = sessions[i];
                if (pids.Contains((int)session.GetProcessID))
                {
                    return session;
                }
            }
            return null;
        }

        public bool SetVolume(float volume) {
            if (this.Session?.State != AudioSessionState.AudioSessionStateActive)
            {
                if ((this.Session = this.getSession()) == null)
                {
                    return false;
                }
            }
            this.Session.SimpleAudioVolume.Volume = volume/ 100;
            return true;
        }
    }
}
