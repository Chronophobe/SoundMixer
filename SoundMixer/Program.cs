using System;
using NAudio.Mixer;
using NAudio.CoreAudioApi.Interfaces;
using NAudio.CoreAudioApi;
using System.Diagnostics;

namespace SoundMixer
{
    class Program
    {
        public static void Main(string[] args)
        {
            /*
            var procs = Process.GetProcessesByName("spotify");
            Console.WriteLine(procs.Length);
            foreach (var p in procs)
            {
                Console.WriteLine(p.MainModule.FileName);
                VolumeMixer.SetApplicationVolume(p.Id, 90f);
                break;
            }
            */

            var deviceEnumerator = new MMDeviceEnumerator();
            var devices = deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
            Console.Write("Process: ");
            var pname = Console.ReadLine();
            var processes = Process.GetProcessesByName(pname);
            if(processes.Length == 0) {
                return;
            }
            var process = processes[0];
            var pid = process.Id;

            foreach(var device in devices)
            {
                if(device.FriendlyName.ToLower().IndexOf("realtek") >= 0)
                {
                    var sessions = device.AudioSessionManager.Sessions;
                    for(int i = 0; i < sessions.Count; i++)
                    {
                        Console.WriteLine("Session " + i);
                        if (sessions[i].GetProcessID == pid)
                        {
                            sessions[i].SimpleAudioVolume.Volume = sessions[i].SimpleAudioVolume.Volume / 2;
                        }
                    }
                }
            }
            
            Console.ReadLine();
        }
    }
}
