using BarRaider.SdTools;
using Newtonsoft.Json;

namespace StreamDeck_KeePass.Domain.Settings
{
    public class CommanderSettings
    {
        public static CommanderSettings CreateDefaultSettings()
        {
            var instance = new CommanderSettings
            {
                CommandDLLPath = string.Empty,
                EntryTitle = string.Empty,
                Field = string.Empty
            };
            return instance;
        }

        [FilenameProperty]
        [JsonProperty(PropertyName = "commandDLLPath")]
        public string CommandDLLPath { get; set; }

        [JsonProperty(PropertyName = "entryTitle")]
        public string EntryTitle { get; set; }

        [JsonProperty(PropertyName = "field")]
        public string Field { get; set; }

        [JsonProperty(PropertyName = "clearTime")]
        public int ClearTime { get; set; }
    }
}