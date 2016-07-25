using System.Windows.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Controls;

namespace clock
{
    interface ITimerWindow
    {
        Dispatcher Dispatcher
        {
            get;
        }

        ContentControl DateText { get; }
        ContentControl TimeText { get; }
        string MessageText { get; set; }

        void SetTitle(string title);
        string GetTitle();
    }
}
