using streamdeck_keepass.Services;
using StreamDeck_KeePass.Domain.Settings;

namespace streamdeck_keepass.Domain
{
    public enum Result
    {
        OK,
        WARNING
    }

    public class KeePassPlugin
    {
        internal readonly IKeePassAction action;

        public KeePassPlugin(GenerateSettings settings) => action = new KeePassGenerate(settings);
        public KeePassPlugin(RetrieveSettings settings) => action = new KeePassRetrieve(settings);

        public Result Invoke()
        {
            var password = action.Invoke();

            if (string.IsNullOrEmpty(password))
            {
                return Result.WARNING;
            }

            ClipboardHelper.SendToClipboard(password);
            return Result.OK;
        }
    }
}
