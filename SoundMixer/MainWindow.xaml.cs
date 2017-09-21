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
using System.IO;

namespace SoundMixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public IConfiguration MixerConfiguration { get; private set; }
        public ApplicationConfiguration AppConfig { get; private set; }

        private Port Port;
        private Mixer Mixer;

        public MainWindow()
        {
            InitializeComponent();
            this.Closing += (s, e) => { this.Port.Close(); };
            this.Port = new Port();
            this.GetPorts(this.ConnectionMenu, null);

            this.AppConfig = ApplicationConfiguration.Load();
            this.Mixer = new Mixer();
            this.InitializeFromConfig();
            
            this.Reload();
            this.Port.OnData += this.ControlVolume;
            
        }

        private void InitializeFromConfig()
        {

            this.MixerConfiguration = new JsonConfiguration();
            if (this.AppConfig.RecentConfigurations.Count > 0 && File.Exists(this.AppConfig.RecentConfigurations[0]))
            {
                loadMixerConfig(this.AppConfig.RecentConfigurations[0]);
            }

            if(this.AppConfig.LastConnectedPort != null && Port.AvailablePorts().Contains(this.AppConfig.LastConnectedPort))
            {
                this.connectPort(this.AppConfig.LastConnectedPort);
            }

            this.GetRecentConfig();
        }

        private void ControlVolume(object sender, SerialDataReceivedEventArgs e)
        {

            var data = ((SerialPort)sender).ReadLine();
            var commandParts = data.Split(new char[] { ':' });

            var volume = long.Parse(commandParts[1]);
            // sliding pot is not linear
            if(commandParts[0] == "0")
            {
                volume = RangeConverter.linearize(
                    volume,
                    new long[] { 0, 46, 513, 977 },
                    new long[] { 45, 512, 976, 1023 },
                    new long[] { 0, 26, 51, 76 },
                    new long[] { 25, 50, 75, 100 });
            }
            else
            {
                volume = RangeConverter.map(volume, 0, 1023, 0, 100);
            }

            var command = new Command(int.Parse(commandParts[0]), (int)volume);

            this.Mixer.SetVolume(command);
        }

        private void GetRecentConfig()
        {
            var recent = this.AppConfig.RecentConfigurations;
            this.RecentMenu.Items.Clear();
            foreach(var config in recent)
            {
                var configItem = new MenuItem();
                configItem.Header = config;
                configItem.Click += this.OnOpenRecent;
                this.RecentMenu.Items.Add(configItem);
            }
            this.RecentMenu.IsEnabled = this.RecentMenu.HasItems;
            this.ConnectionMenu.InvalidateArrange();
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
        }

        private void OnConnect(object sender, RoutedEventArgs e)
        {
            this.ConnectionStatus.Text = "Connecting...";
            var port = (sender as MenuItem).Header as String;
            this.connectPort(port);
        }

        private void connectPort(string port)
        {
            var connected = this.Port.Open(port);
            if (connected)
            {
                this.ConnectionStatus.Text = port;
                this.AppConfig.LastConnectedPort = port;
            }
            else
            {
                this.ConnectionStatus.Text = "Unable to connect";
            }
        }

        private void Reload()
        {
            FirstProfile.Profile = this.MixerConfiguration.Profile;
        }

        private void LoadMixer()
        {
            var profile = this.MixerConfiguration.Profile;
            this.Mixer = new Mixer(profile.Processes);
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
            this.LoadMixer();
        }
        
        private void OnOpenRecent(object sender, RoutedEventArgs e)
        {
            var path = ((MenuItem)sender).Header as String;
            if (!File.Exists(path))
            {
                this.AppConfig.RemoveRecentConfiguration(path);
            }
            else
            {
                this.MixerConfiguration.Load(path);
                this.ConfigurationStatus.Text = path;
                this.LoadMixer();
                this.AppConfig.AddRecentConfiguration(path);
                this.Reload();
            }
            this.GetRecentConfig();
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
                this.loadMixerConfig(path);
                this.AppConfig.AddRecentConfiguration(path);
                this.GetRecentConfig();
            }
        }

        private void loadMixerConfig(string path)
        {
            this.MixerConfiguration.Load(path);
            this.Reload();
            this.ConfigurationStatus.Text = path;
            this.LoadMixer();
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
                this.LoadMixer();
            }
        }
    }
}
