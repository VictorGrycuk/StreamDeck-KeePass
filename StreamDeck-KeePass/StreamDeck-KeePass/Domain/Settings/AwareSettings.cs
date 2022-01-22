using BarRaider.SdTools;
using Newtonsoft.Json;

namespace StreamDeck_KeePass.Domain.Settings
{
    public class AwareSettings
    {
        public static AwareSettings CreateDefaultSettings()
        {
            var instance = new AwareSettings
            {
                DBPath = string.Empty,
                Password = string.Empty,
                KeyFilePath = string.Empty,
                ProcessMapping = string.Empty,
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

        [JsonProperty(PropertyName = "processMapping")]
        public string ProcessMapping { get; set; }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(PropertyName = "clearTime")]
        public int ClearTime { get; set; }
    }
}