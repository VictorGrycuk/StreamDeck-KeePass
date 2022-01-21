using KeePassLib;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using streamdeck_keepass.Services;
using StreamDeck_KeePass.Domain.Settings;
using System.Collections.Generic;
using System.Linq;

namespace streamdeck_keepass.Domain
{
    internal class KeePassAware : IKeePassAction
    {
        private readonly AwareSettings settings;

        public KeePassAware(object objectSettings) => settings = objectSettings as AwareSettings;

        public string Invoke()
        {
            var db = OpenDB(settings);
            var mapping = GetMapping(settings.ProcessMapping);
            if (!mapping.TryGetValue(ProcessHelper.GetActiveProcessFileName(), out string entryId)) return string.Empty;
            var foundEntry = GetKeePassEntry(db, entryId);

            if (foundEntry == null)
            {
                return string.Empty;
            }

            db.Close();

            switch (settings.Field)
            {
                case "Password":
                    return foundEntry.Password;
                case "Username":
                    return foundEntry.Username;
                case "Notes":
                    return foundEntry.Notes;
                case "URL":
                    return foundEntry.URL;
                default:
                    return string.Empty;
            }
        }

        static private PwDatabase OpenDB(AwareSettings settings)
        {
            var conn = new IOConnectionInfo { Path = settings.DBPath };
            var compKey = new CompositeKey();
            compKey.AddUserKey(new KcpPassword(settings.Password));

            if (!string.IsNullOrEmpty(settings.KeyFilePath))
            {
                compKey.AddUserKey(new KcpKeyFile(settings.KeyFilePath));
            }

            var db = new PwDatabase();
            db.Open(conn, compKey, null);

            return db;
        }

        private Dictionary<string, string> GetMapping(string entries)
        {
            var dictionary = new Dictionary<string, string>();
            var lines = entries.Split('\n').ToList();
            lines.ForEach(l => dictionary.Add(l.Split('=')[0], l.Split('=')[1]));

            return dictionary;
        }

        static private KeePassEntry GetKeePassEntry(PwDatabase db, string matchingValue) => (from entry in db.RootGroup.GetEntries(true)
                                                                                             where entry.Strings.ReadSafe("Title") == matchingValue || entry.Uuid.ToHexString() == matchingValue
                                                                                             select new KeePassEntry(entry)).FirstOrDefault();
    }
}
