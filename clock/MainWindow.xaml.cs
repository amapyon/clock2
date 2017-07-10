using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace clock
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window, ITimerWindow
    {
        private static ITimer timer;
        private static List<ITimerWindow> windows = new List<ITimerWindow>();

        public ContentControl DateText
        {
            get { return this.txtDate; }
        }

        public ContentControl TimeText
        {
            get { return this.txtTime; }
        }

        public string MessageText
        {
            get { return this.txtMessage.Text; }
            set { this.txtMessage.Text = value; }
        }

        public static bool Sound { get; internal set; }

        public void SetTitle(String title) { }
        public string GetTitle() { return ""; }

        public MainWindow()
        {
            InitializeComponent();
            windows.Add(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: Window_Loaded");
            this.sldFontSize.Value = this.txtTime.FontSize;
            if (windows.Count < 2)
            {
                timer = Clock.GetInstance(windows);
                timer.Start();
            }
        }

        private void btnClock_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            timer = Clock.GetInstance(windows);
            timer.Start();
        }

        private void btnStopWatch_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            timer = StopWatch.GetInstance(windows);
            timer.Start();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            timer.Start();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
        }

        private void btnCountDown_Click(object sender, RoutedEventArgs e)
        {
            timer.Pause();
            Button b = sender as Button;
            timer = CountDownTimer.GetInstance(windows, (string)(b.Content), (bool)chkThrough.IsChecked);
            timer.Start();
        }

        private void sldFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                this.txtTime.FontSize = (int)e.NewValue;
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        private void btnCountDown_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Button btn = sender as Button;
            InputBox ib = new InputBox();
            ib.txtValue.Text = (string)btn.Content;
            ib.Top = e.GetPosition(this).X;
            ib.Left = e.GetPosition(this).Y;
            ib.ShowDialog();
            btn.Content = ib.txtValue.Text;
        }

        private void mitmDuplicate_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: mitmDuplicate_Click");

            var c = new TimerWindowControler();
            lstWindows.Items.Add(c);

            var w = new TimerWindow(this, c);
            windows.Add(w);
            w.SetTitle("[" + (windows.Count - 1) + "]");

            c.AddWindows(w);
            w.Show();
        }

        public void RemoveWindow(TimerWindowControler controler)
        {
            lstWindows.Items.Remove(controler);
        }

        private void mitmFormat_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: mitmFormat_Click");
            timer.ChangeFormat();
        }

        private void mitmIncrement_Click(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuItem;
            timer.Increment(item.Header.ToString());
        }

        private void txtMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            foreach (ITimerWindow w in windows)
            {
                w.MessageText = this.txtMessage.Text;
            }
        }

        private void mitmTopmost_Click(object sender, RoutedEventArgs e)
        {
            Topmost = mitmTopmost.IsChecked;
        }

        private void mitmSound_Click(object sender, RoutedEventArgs e)
        {
            Sound = mitmSound.IsChecked;
        }

        private void mitmHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            hw.ShowDialog();
        }

        private TimerWindow getTargeWindow(String title)
        {
            TimerWindow targetWindow = null;
            foreach (ITimerWindow w in windows)
            {
                if (w.GetTitle() == title)
                {
                    targetWindow = (TimerWindow)w;
                    return targetWindow;
                }
            }
            return null;
        }

        private void lstWindows_KeyDown(object sender, KeyEventArgs e)
        {
            ((TimerWindowControler)lstWindows.Items[0]).UserControl_KeyDown(sender, e);
        }

        private void lstWindows_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            foreach (System.Windows.Forms.Screen s in System.Windows.Forms.Screen.AllScreens)
            {
                Console.WriteLine("DeviceName:" + s.DeviceName.ToString());
                Console.WriteLine("WorkingArea Location:" + s.WorkingArea.Location + " Size:" + s.WorkingArea.Size);
                Console.WriteLine("Bounds Location:" + s.Bounds.Location + " Size:" + s.Bounds.Size);
            }
            Console.WriteLine("Left:" + this.Left);

            {
                System.Windows.Forms.Screen s = System.Windows.Forms.Screen.FromPoint(new System.Drawing.Point((int)this.Left, (int)this.Top));
                Console.WriteLine("DeviceName:" + s.DeviceName.ToString());
                Console.WriteLine("Bounds Location:" + s.Bounds.Location + " Size:" + s.Bounds.Size);
            }
        }

    }
}
