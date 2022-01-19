using BarRaider.SdTools;
using Newtonsoft.Json;

namespace StreamDeck_KeePass.Domain.Settings
{
    public class RetrieveSettings
    {
        public static RetrieveSettings CreateDefaultSettings()
        {
            var instance = new RetrieveSettings
            {
                DBPath = string.Empty,
                Password = string.Empty,
                KeyFilePath = string.Empty,
                EntryTitle = string.Empty,
                Field = string.Empty
            };
            return instance;
        }

        [FilenameProperty]
        [JsonProperty(PropertyName = "DBPath")]
        public string DBPath { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "keyFilePath")]
        public string KeyFilePath { get; set; }

        [JsonProperty(PropertyName = "entryTitle")]
        public string EntryTitle { get; set; }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }
    }
}