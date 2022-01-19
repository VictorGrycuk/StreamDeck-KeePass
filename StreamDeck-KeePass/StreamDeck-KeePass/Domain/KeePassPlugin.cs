using streamdeck_keepass.Services;
using StreamDeck_KeePass.Domain.Settings;

namespace streamdeck_keepass.Domain
{
    public class KeePassPlugin
    {
        private readonly GenerateSettings generateSettings;
        private readonly RetrieveSettings retrieveSettings;

        public enum Result
        {
            OK,
            WARNING
        }

        public KeePassPlugin(GenerateSettings settings) => generateSettings = settings;
        public KeePassPlugin(RetrieveSettings settings) => retrieveSettings = settings;

        public Result GeneratePassword()
        {
            var password = KeePassGenerate.Invoke(generateSettings);

            if (string.IsNullOrEmpty(password))
            {
                return Result.WARNING;
            }

            ClipboardHelper.SendToClipboard(password);
            return Result.OK;
        }

        public Result RetrievePassword()
        {
            var value = KeePassRetrieve.Invoke(retrieveSettings);

            if (string.IsNullOrEmpty(value))
            {
                return Result.WARNING;
            }
            else
            {
                ClipboardHelper.SendToClipboard(value);
                return Result.OK;
            }
        }
    }
}
