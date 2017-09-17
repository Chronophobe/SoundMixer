using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;

namespace SoundMixer.Configuration
{
    public class JsonConfiguration : IConfiguration
    {

        [JsonProperty]
        public string Path { get; private set; }
        public Profile Profile { get; private set; }

        public JsonConfiguration()
        {
            this.Profile = new Profile(new List<String>(4));
        }

        public JsonConfiguration(String path)
        {
            this.Load(path);
        }

        public void Save()
        {
            var jsonConfig = JsonConvert.SerializeObject(this);
            File.WriteAllText(this.Path, jsonConfig);
        }

        public void SaveAs(String path)
        {
            this.Path = path;
            this.Save();
        }

        public void Load(String path)
        {
            var jsonConfig = File.ReadAllText(path);
            var config =  JsonConvert.DeserializeObject<JsonConfiguration>(jsonConfig);
            this.Path = config.Path;
            this.Profile = config.Profile;
        }
    }
}
