using System;
using System.Collections.Generic;

namespace SoundMixer.Configuration
{
    public class Profile
    {
        public String Name {
            get; set;
        }

        public IList<String> Processes {
            get; private set;
        }

        public Profile(String name, IList<String> processes)
        {
            this.Name = name;
            this.Processes = processes;
        }
    }
}
