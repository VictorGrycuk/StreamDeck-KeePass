using BarRaider.SdTools;
using Newtonsoft.Json.Linq;
using streamdeck_keepass.Actions;
using StreamDeck_KeePass.Domain.Settings;

namespace StreamDeck_KeePass.Actions
{
    [PluginActionId("com.victorgrycuk.keepass.aware")]
    public class ActionAware : ActionBase
    {
        private readonly AwareSettings settings;

        public ActionAware(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            settings = ResolveSettings<AwareSettings>(payload, AwareSettings.CreateDefaultSettings());
            CreatePlugin(settings);
        }

        public override void KeyPressed(KeyPayload payload) => ExecuteAction("Entry not found.");

        public override void ReceivedSettings(ReceivedSettingsPayload payload)
        {
            Tools.AutoPopulateSettings(settings, payload.Settings);
            SaveSettings(JObject.FromObject(settings));
            CreatePlugin(settings);
        }
    }
}