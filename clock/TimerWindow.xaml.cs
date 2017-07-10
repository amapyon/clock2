using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace clock
{
    /// <summary>
    /// TimerWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class TimerWindow : Window, ITimerWindow
    {
        private MainWindow parentWindow;
        private TimerWindowControler controler;

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

        public void SetTitle(String title)
        {
            this.Title = title;
        }

        public string GetTitle()
        {
            return this.Title;
        }

        public TimerWindow(MainWindow parentWindow, TimerWindowControler controler)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            this.controler = controler;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Event: Window_Loaded");
            this.sldFontSize.Value = this.txtTime.FontSize;
        }

        private void txtMessage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                txtMessage.FontSize = e.NewSize.Height * 0.75;
            }
            catch (NullReferenceException exception)
            {
                Console.WriteLine(exception.ToString());
            }
        }

        public MainWindow ParentWindow
        {
            get { return parentWindow; }
        }

        public void setDateTextColor(Brush b)
        {
        }

        public void setTimeTextColor(Brush b)
        {
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Console.WriteLine("[TimerWindow]KeyDown");

            UiUtil.KeyDownHandler(this, e);
        }

        private void txtMessage_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Console.WriteLine("[TimerWindow:txtMessage]PreviewKeyDown");

            UiUtil.KeyDownHandler(this, e);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            parentWindow.RemoveWindow(controler);
        }
    }
}
