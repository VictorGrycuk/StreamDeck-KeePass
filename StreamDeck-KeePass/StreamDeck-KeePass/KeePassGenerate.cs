using BarRaider.SdTools;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using streamdeck_keepass;
using System;
using System.Threading.Tasks;

namespace StreamDeck_KeePass
{
    [PluginActionId("com.victorgrycuk.keepass.generate")]
    public class KeePassGenerate : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                var instance = new PluginSettings
                {
                    Length = 20,
                    UseLowerCase = true,
                    UseUpperCase = true,
                    UseDigits = true,
                    UsePunctuation = true,
                    UseBrackets = true,
                    UseSpecial = true,
                    ExcludeCharacters = string.Empty,
                    CustomPattern = string.Empty
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
        }

        #region Private Members

        private readonly PluginSettings settings;

        #endregion
        public KeePassGenerate(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            if (payload.Settings == null || payload.Settings.Count == 0)
            {
                settings = PluginSettings.CreateDefaultSettings();
            }
            else
            {
                settings = payload.Settings.ToObject<PluginSettings>();
            }
        }

        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");
        }

        public override void KeyPressed(KeyPayload payload)
        {
            try
            {
                var pw = new ProtectedString();
                var profile = new PwProfile();
                profile.CharSet = new PwCharSet();
                profile.CharSet.Clear();

                if (settings.UseLowerCase) profile.CharSet.AddCharSet('l');
                if (settings.UseUpperCase) profile.CharSet.AddCharSet('u');
                if (settings.UseDigits) profile.CharSet.AddCharSet('d');
                if (settings.UsePunctuation) profile.CharSet.AddCharSet('p');
                if (settings.UseBrackets) profile.CharSet.AddCharSet('b');
                if (settings.UseSpecial) profile.CharSet.AddCharSet('s');

                profile.ExcludeLookAlike = settings.ExcludeLookAlike;
                profile.Length = (uint)settings.Length;
                profile.NoRepeatingCharacters = settings.MustOccurAtMostOnce;
                profile.ExcludeCharacters = settings.ExcludeCharacters;

                if (!string.IsNullOrEmpty(settings.CustomPattern))
                {
                    profile.GeneratorType = PasswordGeneratorType.Pattern;
                    profile.PatternPermutePassword = settings.RandomlyPermute;
                    profile.Pattern = settings.CustomPattern;
                }

                PwGenerator.Generate(out pw, profile, null, new CustomPwGeneratorPool());

                if (pw.IsEmpty)
                {
                    Connection.ShowAlert().Wait();
                    Logger.Instance.LogMessage(TracingLevel.WARN, "No password was generated with the given configuration");
                    return;
                }

                Connection.ShowOk().Wait();

                ClipboardHelper.SendToClipboard(pw.ReadString());
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, ex.Message);
            }
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void OnTick() { }

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings();
        }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        #region Private Methods

        private Task SaveSettings()
        {
            return Connection.SetSettingsAsync(JObject.FromObject(settings));
        }

        #endregion
    }
}