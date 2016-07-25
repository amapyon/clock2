using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;

namespace clock
{
    class CountDownTimer : ITimer
    {
        private static List<ITimerWindow> windows = new List<ITimerWindow>();
        private static CountDownTimer instance;

        private Thread timerThread;
        private static int limitSec;
        private static Stopwatch stopwatch;

        private enum ViewMode { S, MMSS };
        private static ViewMode viewMode = ViewMode.MMSS;

        private static bool through = false;

        private CountDownTimer() { }

        public static CountDownTimer GetInstance(List<ITimerWindow> windows, string time, bool through)
        {
            if (instance == null)
            {
                instance = new CountDownTimer();
            }
            CountDownTimer.windows = windows;
            limitSec = ParseTime(time);
            //          w.txtMessage.Text = limit.ToString();
            CountDownTimer.through = through;
            stopwatch = new Stopwatch();
            return instance;
        }

        private static int ParseTime(string time)
        {
            float number = float.Parse(time.Substring(0, time.Length - 1));
            string unit = time.Substring(time.Length - 1);

            switch (unit)
            {
                case "秒":
                    return (int)number;
                case "分":
                    return (int)(number * 60);
                default:
                    return 0;
            }
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

        private void Update()
        {
            while (true)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
                int remainSec = limitSec - (int)elapsed.TotalSeconds;
                int mm = Math.Abs(remainSec / 60);
                int ss = Math.Abs(remainSec % 60);

                foreach (ITimerWindow w in windows)
                {
                    w.Dispatcher.Invoke((Action)(() =>
                    {
                        if (viewMode == ViewMode.S)
                        {
                            w.TimeText.Content = String.Format("{0:0}", remainSec);
                            if (remainSec <= 0)
                            {
                                UiUtil.SetDisableColor(w.TimeText);
                            }
                        }
                        else
                        {

                            w.TimeText.Content = String.Format("{0:0}:{1:00}", mm, ss);
                            if (remainSec <= 0)
                            {
                                UiUtil.SetDisableColor(w.TimeText);
                            }
                        }
                    }
                    ));
                }

                if (!isThrough() && remainSec <= 0)
                {
                    timerThread.Abort();
                    return;
                }
                Thread.Sleep(200);

            }

        }

        protected bool isThrough()
        {
            return through;
        }


        public void Clear() { }

        public void Increment(string time)
        {
            int sec = 0;
            MatchCollection mc = Regex.Matches(time, @"(?<minuts>^\d+)(?<unit>分|秒)(?<inc>延長|短縮)");
            if (mc.Count == 1)
            {
                sec = int.Parse(mc[0].Groups["minuts"].Value);
                if (mc[0].Groups["unit"].Value == "分")
                {
                    sec *= 60;
                }

                if (mc[0].Groups["inc"].Value == "短縮")
                {
                    sec *= -1;
                }
                Console.WriteLine(sec);
            }

            limitSec += sec;
        }

        public void ChangeFormat()
        {
            if (viewMode == ViewMode.MMSS)
            {
                viewMode = ViewMode.S;
            }
            else
            {
                viewMode = ViewMode.MMSS;
            }
        }

    }
}
