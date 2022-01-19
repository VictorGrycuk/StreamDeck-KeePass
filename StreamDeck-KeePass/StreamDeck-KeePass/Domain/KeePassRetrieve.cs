using KeePassLib;
using KeePassLib.Keys;
using KeePassLib.Serialization;
using StreamDeck_KeePass.Domain.Settings;
using System.Linq;

namespace streamdeck_keepass.Domain
{
    internal class KeePassRetrieve : IKeePassAction
    {
        private readonly RetrieveSettings settings;

        public KeePassRetrieve(object objectSettings) => settings = objectSettings as RetrieveSettings;

        public string Invoke()
        {
            var db = OpenDB(settings);
            var foundEntry = GetKeePassEntry(db, settings.EntryTitle);

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

        static private PwDatabase OpenDB(RetrieveSettings settings)
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

        static private KeePassEntry GetKeePassEntry(PwDatabase db, string matchingValue) => (from entry in db.RootGroup.GetEntries(true)
                                                                                             where entry.Strings.ReadSafe("Title") == matchingValue
                                                                                             select new KeePassEntry(entry)).FirstOrDefault();
    }
}
