using System;
using System.Collections.Generic;

namespace SoundMixer.Configuration
{
    public interface IConfiguration
    {
        String Path {
            get;
        }

        IList<Profile> Profiles {
            get;
        }

        void Load(String path);
        void Save();
        void SaveAs(String path);
    }
}
