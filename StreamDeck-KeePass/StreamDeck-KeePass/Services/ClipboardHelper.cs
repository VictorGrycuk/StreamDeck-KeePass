using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace streamdeck_keepass.Services
{
    public static class ClipboardHelper
    {
        internal static void SendToClipboard(string text, int clearDelay)
        {
            ExecuteThread(new Thread(() => Clipboard.SetText(text)));
            if (clearDelay > 0) ClearClipboard(clearDelay);
        }

        private static void ClearClipboard(int delay)
        {
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(delay * 1000);
                ExecuteThread(new Thread(() => Clipboard.Clear()));
            });
        }

        private static void ExecuteThread(Thread thread)
        {
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = false;
            thread.Start();
        }
    }
}
