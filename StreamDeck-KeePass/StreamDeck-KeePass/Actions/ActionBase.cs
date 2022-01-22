using BarRaider.SdTools;
using Newtonsoft.Json.Linq;
using streamdeck_keepass.Domain;
using StreamDeck_KeePass.Domain.Settings;
using System.Threading.Tasks;

namespace streamdeck_keepass.Actions
{
    abstract public class ActionBase : PluginBase
    {
        private KeePassPlugin plugin;

        public ActionBase(SDConnection connection, InitialPayload payload) : base(connection, payload) { }

        internal void CreatePlugin(GenerateSettings settings) => plugin = new KeePassPlugin(settings);

        internal void CreatePlugin(RetrieveSettings settings) => plugin = new KeePassPlugin(settings);

        internal void CreatePlugin(AwareSettings settings) => plugin = new KeePassPlugin(settings);

        internal void CreatePlugin(CommanderSettings settings) => plugin = new KeePassPlugin(settings);

        public override void Dispose() => Logger.Instance.LogMessage(TracingLevel.INFO, "Destructor called");

        public void ExecuteAction(string logMessage)
        {
            try
            {
                var result = plugin.Invoke();

                if (result == Result.WARNING)
                {
                    Connection.ShowAlert().Wait();
                    Logger.Instance.LogMessage(TracingLevel.WARN, logMessage);
                }
                else
                {
                    Connection.ShowOk().Wait();
                }
            }
            catch (System.Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.INFO, ex.Message);
            }
        }

        public override void KeyReleased(KeyPayload payload) { }

        public override void OnTick() { }

        public override void ReceivedGlobalSettings(ReceivedGlobalSettingsPayload payload) { }

        internal T ResolveSettings<T>(InitialPayload payload, object defaultConfig) where T : class
        {
            return payload.Settings == null || payload.Settings.Count == 0
                  ? defaultConfig as T
                  : payload.Settings.ToObject<T>();
        }

        internal Task SaveSettings(JObject settings) => Connection.SetSettingsAsync(settings);
    }
}
