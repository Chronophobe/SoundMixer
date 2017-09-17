using SoundMixer.Configuration;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SoundMixer
{
    /// <summary>
    /// Interaction logic for ProfileControl.xaml
    /// </summary>
    public partial class ProfileControl : UserControl
    {
        public ProfileControl()
        {
            InitializeComponent();
        }

        private Profile profile;
        public Profile Profile {
            get {
                return this.profile;
            }
            set {
                this.profile = value;
                if (value.Processes.Count > 0)
                    ProcessOne.Text = value.Processes[0];
                else
                    ProcessOne.Text = "";
                if (value.Processes.Count > 1)
                    ProcessTwo.Text = value.Processes[1];
                else
                    ProcessTwo.Text = "";
                if (value.Processes.Count > 2)
                    ProcessThree.Text = value.Processes[2];
                else
                    ProcessThree.Text = "";
                if (value.Processes.Count > 3)
                    ProcessFour.Text = value.Processes[3];
                else
                    ProcessFour.Text = "";
            }
        }

        private void UpdateProcessOne(object sender, TextChangedEventArgs e)
        {
            if (this.Profile.Processes.Count == 0)
            {
                this.Profile.Processes.Add(ProcessOne.Text);
            }
            else
            {
                this.Profile.Processes[0] = ProcessOne.Text;
            }
        }

        private void UpdateProcessTwo(object sender, TextChangedEventArgs e)
        {
            if (this.Profile.Processes.Count == 1)
            {
                this.Profile.Processes.Add(ProcessTwo.Text);
            }
            else
            {
                this.Profile.Processes[1] = ProcessTwo.Text;
            }
        }

        private void UpdateProcessThree(object sender, TextChangedEventArgs e)
        {
            if (this.Profile.Processes.Count == 2)
            {
                this.Profile.Processes.Add(ProcessThree.Text);
            }
            else
            {
                this.Profile.Processes[2] = ProcessThree.Text;
            }
        }

        private void UpdateProcessFour(object sender, TextChangedEventArgs e)
        {
            if (this.Profile.Processes.Count == 3)
            {
                this.Profile.Processes.Add(ProcessFour.Text);
            }
            else
            {
                this.Profile.Processes[3] = ProcessFour.Text;
            }
        }
    }
}
