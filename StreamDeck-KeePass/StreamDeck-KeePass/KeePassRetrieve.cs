﻿using BarRaider.SdTools;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using streamdeck_keepass;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StreamDeck_KeePass
{
    [PluginActionId("com.victorgrycuk.keepass.retrieve")]
    public class KeePassRetrieve : PluginBase
    {
        private class PluginSettings
        {
            public static PluginSettings CreateDefaultSettings()
            {
                var instance = new PluginSettings
                {
                    DBPath = string.Empty,
                    Password = string.Empty,
                    EntryTitle = string.Empty
                };
                return instance;
            }

            [FilenameProperty]
            [JsonProperty(PropertyName = "DBPath")]
            public string DBPath { get; set; }

            [JsonProperty(PropertyName = "Password")]
            public string Password { get; set; }

            [JsonProperty(PropertyName = "entryTitle")]
            public string EntryTitle { get; set; }
        }

        #region Private Members

        private PluginSettings settings;

        #endregion
        public KeePassRetrieve(SDConnection connection, InitialPayload payload) : base(connection, payload)
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
                var conn = new IOConnectionInfo { Path = settings.DBPath };
                var compKey = new CompositeKey();
                compKey.AddUserKey(new KcpPassword(settings.Password));
                var db = new KeePassLib.PwDatabase();
                db.Open(conn, compKey, null);
                var entryList = from entry in db.RootGroup.GetEntries(true)
                                select new
                                {
                                    Group = entry.ParentGroup.Name,
                                    Title = entry.Strings.ReadSafe("Title"),
                                    Username = entry.Strings.ReadSafe("UserName"),
                                    Password = entry.Strings.ReadSafe("Password"),
                                    URL = entry.Strings.ReadSafe("URL"),
                                    Notes = entry.Strings.ReadSafe("Notes")

                                };
                var pw = entryList.Where(x => x.Title == settings.EntryTitle).FirstOrDefault().Password;
                ClipboardHelper.SendToClipboard(pw);
                db.Close();
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