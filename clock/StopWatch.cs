using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace clock
{
    class StopWatch : ITimer
    {
        private static List<ITimerWindow> windows = new List<ITimerWindow>();
        private static StopWatch instance;

        private Thread timerThread;
        private static Stopwatch stopwatch;

        public StopWatch()
        {
        }

        public static StopWatch GetInstance(List<ITimerWindow> windows)
        {
            if (instance == null)
            {
                instance = new StopWatch();
            }
            StopWatch.windows = windows;
            stopwatch = new Stopwatch();
            return instance;
        }

        public void Start()
        {
            foreach (ITimerWindow w in windows)
            {
                UiUtil.SetEnableColor(w.TimeText);
                //                w.Closing += (_, __) => Pause();
            }
            timerThread = new Thread(() =>
            {
                Update();
            });
            stopwatch.Start();
            timerThread.Start();
        }

        public void Pause()
        {
            foreach (ITimerWindow w in windows)
            {
                UiUtil.SetDisableColor(w.TimeText);
            }
            stopwatch.Stop();
            timerThread.Abort();
        }

        public void Clear()
        {
        }

        private void Update()
        {
            while (true)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
                int mm = elapsed.Minutes;
                int ss = elapsed.Seconds;

                foreach (ITimerWindow w in windows)
                {
                    try
                    {
                        w.Dispatcher.Invoke(() =>
                        {
                            w.TimeText.Content = String.Format("{0:0}:{1:00}", mm, ss);
                        });
                    }
                    catch (TaskCanceledException ex)
                    {
                        return;
                    }
                }

                Thread.Sleep(200);
            }
        }

        public void Increment(string time) { }
        public void ChangeFormat() { }

    }
}
