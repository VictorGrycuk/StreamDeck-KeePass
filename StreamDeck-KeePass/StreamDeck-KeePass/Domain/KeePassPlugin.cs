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
        internal readonly int clearDelay;

        public KeePassPlugin(GenerateSettings settings)
        {
            action = new KeePassGenerate(settings);
            clearDelay = settings.ClearTime;
        }

        public KeePassPlugin(RetrieveSettings settings)
        {
            action = new KeePassRetrieve(settings);
            clearDelay = settings.ClearTime;
        }

        public Result Invoke()
        {
            var password = action.Invoke();

            if (string.IsNullOrEmpty(password))
            {
                return Result.WARNING;
            }

            ClipboardHelper.SendToClipboard(password, clearDelay);

            return Result.OK;
        }
    }
}
