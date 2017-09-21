using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace SoundMixer.Configuration
{
    public class ApplicationConfiguration
    {

        public static string Path { get {
                return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\soundmixer.json";
        } }

        public int RecentListSize { get; set; }

        private String lastPort;

        [JsonProperty]
        public IList<String> RecentConfigurations { get; private set; }

        [JsonProperty]
        public String LastConnectedPort {
            get { return this.lastPort; }
            set { this.lastPort = value; this.Save(); }
        }

        public ApplicationConfiguration(int recentListSize = 5)
        {
            this.RecentConfigurations = new List<string>();
            this.RecentListSize = recentListSize;
        }

        public void Save()
        {
            var jsonConfig = JsonConvert.SerializeObject(this);
            File.WriteAllText(ApplicationConfiguration.Path, jsonConfig);
        }

        public static ApplicationConfiguration Load()
        {
            if (!File.Exists(ApplicationConfiguration.Path)) return new ApplicationConfiguration();

            var jsonConfig = File.ReadAllText(ApplicationConfiguration.Path);
            var config = JsonConvert.DeserializeObject<ApplicationConfiguration>(jsonConfig);
            return config;
        }

        public void AddRecentConfiguration(string path)
        {
            var newRecent = new List<String>(this.RecentListSize);
            newRecent.Add(path);
            foreach(var config in this.RecentConfigurations)
            {
                if (newRecent.Count >= this.RecentListSize) break;
                if (config == path) continue;

                newRecent.Add(config);
            }
            this.RecentConfigurations = newRecent;
            this.Save();
        }

        public void RemoveRecentConfiguration(string path)
        {
            this.RecentConfigurations.Remove(path);
            this.Save();
        }
    }
}
