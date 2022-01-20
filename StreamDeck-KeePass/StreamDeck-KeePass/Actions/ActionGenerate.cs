using BarRaider.SdTools;
using Newtonsoft.Json.Linq;
using streamdeck_keepass.Actions;
using StreamDeck_KeePass.Domain.Settings;

namespace StreamDeck_KeePass.Actions
{
    [PluginActionId("com.victorgrycuk.keepass.generate")]
    public class ActionGenerate : ActionBase
    {
        private readonly GenerateSettings settings;

        public ActionGenerate(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            settings = ResolveSettings<GenerateSettings>(payload, GenerateSettings.CreateDefaultSettings());
            CreatePlugin(settings);
        }

        public override void KeyPressed(KeyPayload payload) => ExecuteAction("No password was generated with the given configuration.");

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings(JObject.FromObject(settings));
            CreatePlugin(settings);
        }
    }
}