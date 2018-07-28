using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace clock
{
    class CountDownTimer : ITimer
    {
        private static List<ITimerWindow> windows = new List<ITimerWindow>();
        private static CountDownTimer instance;

        private Thread timerThread;
        private static int limitSec;
        private static Stopwatch stopwatch;

        private enum ViewFormat { S, MMSS };
        private static ViewFormat viewFormat = ViewFormat.MMSS;

        private static bool through = false;

        private static Alarm alarm;

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
            alarm = new Alarm();

            return instance;
        }

        private static int ParseTime(string time)
        {
            var number = float.Parse(time.Substring(0, time.Length - 1));
            var unit = time.Substring(time.Length - 1);

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
                var elapsed = stopwatch.Elapsed;
                var remainSec = limitSec - (int)elapsed.TotalSeconds;
                var mm = Math.Abs(remainSec / 60);
                var ss = Math.Abs(remainSec % 60);

                foreach (ITimerWindow w in windows)
                {
                    try
                    {
                        w.Dispatcher.Invoke(() =>
                        {
                            if (viewFormat == ViewFormat.S)
                            {
                                w.TimeText.Content = String.Format("{0:0}", remainSec);
                            }
                            else
                            {
                                w.TimeText.Content = String.Format("{0:0}:{1:00}", mm, ss);
                            }

                            if (remainSec <= 0)
                            {
                                UiUtil.SetDisableColor(w.TimeText);
                            }

                            if (remainSec == 0 && MainWindow.Sound)
                            {
                                alarm.RingOnce();

                            }
                        });
                    }
                    catch (TaskCanceledException ex)
                    {
                        return;
                    }
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
            var sec = 0;
            var mc = Regex.Matches(time, @"(?<minuts>^\d+)(?<unit>分|秒)(?<inc>延長|短縮)");
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
            if (viewFormat == ViewFormat.MMSS)
            {
                viewFormat = ViewFormat.S;
            }
            else
            {
                viewFormat = ViewFormat.MMSS;
            }
        }

    }
}
