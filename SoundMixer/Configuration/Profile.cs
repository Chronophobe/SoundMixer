using System;
using System.Collections.Generic;

namespace SoundMixer.Configuration
{
    public class Profile
    {
        public IList<String> Processes {
            get; private set;
        }

        public Profile(IList<String> processes)
        {
            this.Processes = processes;
        }
    }
}
