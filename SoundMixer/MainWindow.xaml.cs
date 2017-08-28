using Microsoft.Win32;
using SoundMixer.Configuration;
using SoundMixer.Controller;
using System;
using System.Collections.Generic;
using System.Windows;

namespace SoundMixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public IConfiguration MixerConfiguration { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            this.MixerConfiguration = new JsonConfiguration();
            for (int i = 0; i < 3; i++) {
                this.MixerConfiguration.Profiles.Add(new Profile("", new List<String>(3)));
            }
            this.Reload();
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
            }
        }
    }
}
