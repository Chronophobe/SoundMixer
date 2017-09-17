using System;
using System.Collections.Generic;

namespace SoundMixer.Configuration
{
    public interface IConfiguration
    {
        String Path {
            get;
        }

        Profile Profile {
            get;
        }

        void Load(String path);
        void Save();
        void SaveAs(String path);
    }
}
