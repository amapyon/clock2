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
            get
            {
                return this.txtDate;
            }
        }

        public ContentControl TimeText
        {
            get
            {
                return this.txtTime;
            }
        }

        public string MessageText
        {
            get
            {
                return this.txtMessage.Text;
            }

            set
            {
                this.txtMessage.Text = value;
            }
        }

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
            Button b = (Button)sender;
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
            Button btn = (Button)sender;
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
            TimerWindow w = new TimerWindow();
            windows.Add(w);
            w.SetTitle("[" + (windows.Count - 1) + "]");
            this.lstWindows.Items.Add(w.GetTitle());

            w.Show();
        }

        private void mitmFormat_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: mitmFormat_Click");
            timer.ChangeFormat();
        }

        private void mitmIncrement_Click(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
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
            this.Topmost = mitmTopmost.IsChecked;
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
            Console.WriteLine(lstWindows.SelectedItem.ToString());
            Console.WriteLine("KeyDown:[" + Keyboard.Modifiers + "]+" + e.Key);

            TimerWindow targetWindow = getTargeWindow(this.lstWindows.SelectedItem.ToString());

            System.Drawing.Point p = new System.Drawing.Point((int)targetWindow.Left, (int)targetWindow.Top);
            System.Drawing.Rectangle area = System.Windows.Forms.Screen.GetWorkingArea(p);
            Console.WriteLine("WorkingArea:" + area);

            System.Windows.PresentationSource s = PresentationSource.FromVisual(targetWindow);
            double scaleX = s.CompositionTarget.TransformToDevice.M11;
            double scaleY = s.CompositionTarget.TransformToDevice.M22;
            Console.WriteLine("Sclae:" + scaleX + ", " + scaleY);

            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
            {
                if (e.Key == Key.Left)
                {
                    targetWindow.Left = area.Left / scaleX;
                }
                else if (e.Key == Key.Right)
                {
                    targetWindow.Left = ((area.X + area.Width) / scaleX) - targetWindow.Width;
                }
                else if (e.Key == Key.Up)
                {
                    targetWindow.Top = area.Y / scaleY;
                }
                else if (e.Key == Key.Down)
                {
                    targetWindow.Top = ((area.Y + area.Height) / scaleY) - targetWindow.Height;
                }

            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
            {
                if (e.Key == Key.Left)
                {
                    targetWindow.Width -= 3;
                }
                else if (e.Key == Key.Right)
                {
                    targetWindow.Width += 3;

                }
                else if (e.Key == Key.Up)
                {
                    targetWindow.Height -= 3;
                }
                else if (e.Key == Key.Down)
                {
                    targetWindow.Height += 3;
                }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
            {
                if (e.Key == Key.Left)
                {
                    targetWindow.Left -= 3;
                }
                else if (e.Key == Key.Right)
                {
                    targetWindow.Left += 3;

                }
                else if (e.Key == Key.Up)
                {
                    targetWindow.Top -= 3;
                }
                else if (e.Key == Key.Down)
                {
                    targetWindow.Top += 3;
                }
            }
            else
            {
                if (e.Key == Key.Left)
                {
                    targetWindow.sldFontSize.Value -= 1;
                }
                else if (e.Key == Key.Right)
                {
                    targetWindow.sldFontSize.Value += 1;
                }
            }
            Console.WriteLine("Top:" + targetWindow.Top + ", Left:" + targetWindow.Left + ", Width:" + targetWindow.Width + ", Height:" + targetWindow.Height);
        }

        private void mitmTopOfMost_Click(object sender, RoutedEventArgs e)
        {
            TimerWindow targetWindow = getTargeWindow(this.lstWindows.SelectedItem.ToString());
            if (targetWindow != null)
            {
                targetWindow.Topmost = mitmTopOfMost.IsChecked;
            }

        }

        private void mitmMinimize_Click(object sender, RoutedEventArgs e)
        {
            TimerWindow targetWindow = getTargeWindow(this.lstWindows.SelectedItem.ToString());
            targetWindow.WindowState = WindowState.Minimized;
        }

        private void mitmNormal_Click(object sender, RoutedEventArgs e)
        {
            TimerWindow targetWindow = getTargeWindow(this.lstWindows.SelectedItem.ToString());
            targetWindow.WindowState = WindowState.Normal;
        }
    }
}
