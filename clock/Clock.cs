using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace clock
{
    public delegate void Update();

    class Clock : ITimer
    {
        private static List<ITimerWindow> windows;
        private Thread timerThread;

        private static Clock instance;

        private Clock()
        {
        }

        public static Clock GetInstance(List<ITimerWindow> windows)
        {
            if (instance == null)
            {
                instance = new Clock();
            }
            Clock.windows = windows;

            return instance;
        }

        public void Start()
        {
            foreach (ITimerWindow w in windows)
            {
                UiUtil.SetEnableColor(w.TimeText);
            }
            timerThread = new Thread(() =>
            {
                Update();
            });
            timerThread.Start();
        }

        public void Pause()
        {
            Console.WriteLine("Method: Clock Pause()");
            foreach (ITimerWindow w in windows)
            {
                UiUtil.SetDisableColor(w.TimeText);
            }
            timerThread.Abort();
        }

        private void Update()
        {
            while (true)
            {
                foreach (ITimerWindow w in windows)
                {
                    w.Dispatcher.Invoke((Action)(() =>
                    {
                        w.DateText.Content = DateTime.Now.ToString("yyyy年M月d日(ddd)");
                        w.TimeText.Content = DateTime.Now.ToString("H:mm:ss");
                    }
                    ));
                }
                Thread.Sleep(200);
            }
        }

        public void Clear() { }
        public void Increment(string time) {}
        public void ChangeFormat() {}
    }
}
