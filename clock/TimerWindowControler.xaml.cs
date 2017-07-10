using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace clock
{
    /// <summary>
    /// TimerWindowControler.xaml の相互作用ロジック
    /// </summary>
    public partial class TimerWindowControler : UserControl
    {
        TimerWindow w;

        public TimerWindowControler()
        {
            InitializeComponent();
        }

        internal void AddWindows(TimerWindow w)
        {
            this.w = w;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            w.Topmost = true;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            w.Topmost = false;
        }

        private void btnMin_Click(object sender, RoutedEventArgs e)
        {
            w.WindowState = WindowState.Minimized;
        }

        private void btnNormal_Click(object sender, RoutedEventArgs e)
        {
            w.WindowState = WindowState.Normal;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            w.Close();
            w.ParentWindow.RemoveWindow(this);
        }

        public void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown:[" + Keyboard.Modifiers + "]+" + e.Key);

            UiUtil.KeyDownHandler(w, e);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Point p = new System.Drawing.Point((int)w.Left, (int)w.Top);
            System.Windows.Forms.Screen s = System.Windows.Forms.Screen.FromPoint(p);

            Console.WriteLine("Width:" + s.Bounds.Width);

            w.Left += s.Bounds.Width;

            Console.WriteLine("Location Left: " + w.Left + " Top:" + w.Top);
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Point p = new System.Drawing.Point((int)w.Left, (int)w.Top);
            System.Windows.Forms.Screen s = System.Windows.Forms.Screen.FromPoint(p);

            Console.WriteLine("Width:" + s.Bounds.Width);

            w.Left -= s.Bounds.Width;

            Console.WriteLine("Location Left: " + w.Left + " Top:" + w.Top);
        }

    }
}
