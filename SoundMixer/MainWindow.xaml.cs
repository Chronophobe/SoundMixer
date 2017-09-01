using Microsoft.Win32;
using SoundMixer.Configuration;
using SoundMixer.Controller;
using SoundMixer.Serial;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.IO.Ports;
using NAudio.CoreAudioApi;
using System.Linq;
using System.Diagnostics;

namespace SoundMixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public IConfiguration MixerConfiguration { get; private set; }
        private Port Port;

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += (s, e) => { this.Port.Close(); };
            this.Port = new Port();
            this.GetPorts(this.ConnectionMenu, null);

            this.MixerConfiguration = new JsonConfiguration();
            for (int i = 0; i < 3; i++) {
                this.MixerConfiguration.Profiles.Add(new Profile("", new List<String>(3)));
            }
            this.Reload();
            this.Port.OnData += this.ControlVolume;
        }

        private void ControlVolume(object sender, SerialDataReceivedEventArgs e)
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia | Role.Console);
            var a = device.AudioEndpointVolume.VolumeRange;
            var data = ((SerialPort)sender).ReadLine();
            var volume = long.Parse(new String(data.Where(Char.IsDigit).ToArray()));

            volume = this.linearize(
                volume,
                new long[] { 0, 46, 513, 977 },
                new long[] { 45, 512, 976, 1023},
                new long[] { 0, 26, 51, 76 },
                new long[] { 25, 50, 75, 100 } );
            device.AudioEndpointVolume.MasterVolumeLevelScalar = ((float) volume) / 100;
        }

        public long linearize(long val, long[] in_min, long[] in_max, long[] out_min, long[] out_max)
        {
            for(int i = 0; i < in_min.Length; i++)
            {
                if(val <= in_max[i])
                {
                    return this.map(val, in_min[i], in_max[i], out_min[i], out_max[i]);
                }
            }
            return val;
        }

        public long map(long val, long in_min, long in_max, long out_min, long out_max)
        {
            return (val - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
        private void GetPorts(object sender, RoutedEventArgs e)
        {
            var ports = Port.AvailablePorts();
            if(ports.Count == 0) {
                this.ConnectionStatus.Text = "No devices available";
            }
            var wasEmpty = !this.ConnectionMenu.HasItems;
            this.ConnectionMenu.Items.Clear();
            foreach(var port in ports)
            {
                var portItem = new MenuItem();
                portItem.Header = port;
                portItem.Click += this.OnConnect;
                this.ConnectionMenu.Items.Add(portItem);
            }
            this.ConnectionMenu.InvalidateArrange();
            if (wasEmpty && this.ConnectionMenu.HasItems)
            {
                this.ConnectionMenu.SubmenuOpened -= GetPorts;
                this.ConnectionMenu.IsSubmenuOpen = true;
                this.ConnectionMenu.SubmenuOpened += GetPorts;
            }
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            this.ConnectionStatus.Text = "Connecting...";
            var port = (sender as MenuItem).Header as String;
            var connected = this.Port.Open(port);
            if (connected)
            {
                this.ConnectionStatus.Text = port;
            }
            else
            {
                this.ConnectionStatus.Text = "Unable to connect";
            }
        }

        private void Reload()
        {
            var count = this.MixerConfiguration.Profiles.Count;
            if (count < 3)
            {
                for (int i = 0; i < 3 - count; i++){
                    this.MixerConfiguration.Profiles.Add(new Profile("", new List<String>(3)));
                }
            }
            FirstProfile.Profile = this.MixerConfiguration.Profiles[0];
            SecondProfile.Profile = this.MixerConfiguration.Profiles[1];
            ThirdProfile.Profile = this.MixerConfiguration.Profiles[2];
        }

        private void OnNew(object sender, RoutedEventArgs e)
        {
            this.MixerConfiguration = new JsonConfiguration();
            this.Reload();
            this.ConfigurationStatus.Text = "New configuration"; // todo: move magic strings to a resource file
        }

        private void OnSave(object sender, RoutedEventArgs e)
        {
            if(this.MixerConfiguration.Path == null)
            {
                this.OnSaveAs(sender, e);
            }
            else
            {
                this.MixerConfiguration.Save();
            }
            
        }

        private void OnOpen(object sender, RoutedEventArgs e)
        {
            var dialogue = new OpenFileDialog()
            {
                DefaultExt = ".json",
                Filter = "JSON document|*.json"
            };
            if (dialogue.ShowDialog() == true)
            {
                var path = dialogue.FileName;
                this.MixerConfiguration.Load(path);
                this.Reload();
                this.ConfigurationStatus.Text = path;
            }
        }

        private void OnSaveAs(object sender, RoutedEventArgs e)
        {
            var dialogue = new SaveFileDialog()
            {
                FileName = "my_mixer",
                DefaultExt = ".json",
                Filter = "JSON document|*.json"
            };

            if (dialogue.ShowDialog() == true)
            {
                var path = dialogue.FileName;
                this.MixerConfiguration.SaveAs(path);
                this.ConfigurationStatus.Text = path;
            }
        }
    }
}
