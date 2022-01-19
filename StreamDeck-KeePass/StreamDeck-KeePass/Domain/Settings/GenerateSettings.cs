using Newtonsoft.Json;

namespace StreamDeck_KeePass.Domain.Settings
{
    public class GenerateSettings
    {
        public static GenerateSettings CreateDefaultSettings()
        {
            var instance = new GenerateSettings
            {
                Length = 20,
                UseLowerCase = true,
                UseUpperCase = true,
                UseDigits = true,
                UsePunctuation = true,
                UseBrackets = true,
                UseSpecial = true,
                ExcludeCharacters = string.Empty,
                CustomPattern = string.Empty,
                ClearTime = 0
            };
            return instance;
        }

        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }

        [JsonProperty(PropertyName = "useLowerCase")]
        public bool UseLowerCase { get; set; }

        [JsonProperty(PropertyName = "useUpperCase")]
        public bool UseUpperCase { get; set; }

        [JsonProperty(PropertyName = "useDigits")]
        public bool UseDigits { get; set; }

        [JsonProperty(PropertyName = "usePunctuation")]
        public bool UsePunctuation { get; set; }

        [JsonProperty(PropertyName = "useBrackets")]
        public bool UseBrackets { get; set; }

        [JsonProperty(PropertyName = "useSpecial")]
        public bool UseSpecial { get; set; }

        [JsonProperty(PropertyName = "excludeLookAlike")]
        public bool ExcludeLookAlike { get; set; }

        [JsonProperty(PropertyName = "mustOccurAtMostOnce")]
        public bool MustOccurAtMostOnce { get; set; }

        [JsonProperty(PropertyName = "excludeCharacters")]
        public string ExcludeCharacters { get; set; }

        [JsonProperty(PropertyName = "customPattern")]
        public string CustomPattern { get; set; }

        [JsonProperty(PropertyName = "randomlyPermute")]
        public bool RandomlyPermute { get; set; }

        [JsonProperty(PropertyName = "clearTime")]
        public int ClearTime { get; set; }
    }
}