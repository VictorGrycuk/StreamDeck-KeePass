using StreamDeck_KeePass.Domain.Settings;

namespace streamdeck_keepass.Domain
{
    internal class KeePassCommander : IKeePassAction
    {
        private readonly CommanderSettings settings;

        public KeePassCommander(object objectSettings)
        {
            settings = objectSettings as CommanderSettings;

            if (!KeePassCommand.KeePassEntry.IsInitialized())
                KeePassCommand.KeePassEntry.Initialize(settings.CommandDLLPath);
        }

        public string Invoke()
        {
            var entry = KeePassCommand.KeePassEntry.getfirst(settings.EntryTitle);

            if (entry == null) return string.Empty;

            switch (settings.Field)
            {
                case "Password":
                    return entry.Password;
                case "Username":
                    return entry.Username;
                case "Notes":
                    return entry.Notes;
                case "URL":
                    return entry.Url;
                default:
                    return string.Empty;
            }
        }
    }
}
