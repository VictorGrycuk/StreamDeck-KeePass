using BarRaider.SdTools;
using Newtonsoft.Json.Linq;
using streamdeck_keepass.Domain;
using StreamDeck_KeePass.Domain.Settings;
using System;
using System.Threading.Tasks;

namespace StreamDeck_KeePass
{
    [PluginActionId("com.victorgrycuk.keepass.retrieve")]
    public partial class ActionRetrieve : PluginBase
    {

        private readonly KeePassPlugin plugin;
        private readonly RetrieveSettings settings;

        public ActionRetrieve(SDConnection connection, InitialPayload payload) : base(connection, payload)
        {
            settings = payload.Settings == null || payload.Settings.Count == 0
                ? RetrieveSettings.CreateDefaultSettings()
                : payload.Settings.ToObject<RetrieveSettings>();

            plugin = new KeePassPlugin(settings);
        }

        public override void Dispose()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Destructor called");
        }

        public override void KeyPressed(KeyPayload payload)
        {
            try
            {
                var result = plugin.Invoke();

                if (result == Result.WARNING)
                {
                    Connection.ShowAlert().Wait();
                    Logger.Instance.LogMessage(TracingLevel.WARN, "No password was generated with the given configuration");
                    return;
                }
                else
                {
                    Connection.ShowOk().Wait();
                }
            }
            catch (Exception ex)
            {
                Connection.ShowAlert();
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