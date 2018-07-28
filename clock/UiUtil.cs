using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace clock
{
    class UiUtil
    {

        public static void SetEnableColor(Control control)
        {
            control.Foreground = new SolidColorBrush(System.Windows.Media.Colors.White);
        }

        public static void SetDisableColor(Control control)
        {
            control.Foreground = new SolidColorBrush(System.Windows.SystemColors.ControlDarkDarkColor);
        }

        private const int THICK = 3;

        public static void KeyDownHandler(TimerWindow w, KeyEventArgs e)
        {
            Console.WriteLine("KeyDown:[" + Keyboard.Modifiers + "]+" + e.Key);

            System.Windows.PresentationSource ps = System.Windows.PresentationSource.FromVisual(w);
            double scaleX = ps.CompositionTarget.TransformToDevice.M11;
            double scaleY = ps.CompositionTarget.TransformToDevice.M22;
            Console.WriteLine("Scale:" + scaleX + ", " + scaleY);

            System.Drawing.Point p = new System.Drawing.Point((int)(w.Left * scaleY), (int)(w.Top * scaleX));
            System.Drawing.Rectangle area = System.Windows.Forms.Screen.GetWorkingArea(p);
            Console.WriteLine("WorkingArea:" + area);


            if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None && (Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
            {
                if (e.Key == Key.Left)
                {
                    w.Left = area.Left / scaleX;
                }
                else if (e.Key == Key.Right)
                {
                    w.Left = ((area.Location.X + area.Width) / scaleX) - w.Width;
                }
                else if (e.Key == Key.Up)
                {
                    w.Top = area.Y / scaleY;
                }
                else if (e.Key == Key.Down)
                {
                    w.Top = ((area.Y + area.Height) / scaleY) - w.Height;
                }

            }
            else if ((Keyboard.Modifiers & ModifierKeys.Control) != ModifierKeys.None)
            {
                if (e.Key == Key.Left)
                {
                    w.Width -= THICK;
                }
                else if (e.Key == Key.Right)
                {
                    w.Width += THICK;

                }
                else if (e.Key == Key.Up)
                {
                    w.Height -= THICK;
                }
                else if (e.Key == Key.Down)
                {
                    w.Height += THICK;
                }
            }
            else if ((Keyboard.Modifiers & ModifierKeys.Shift) != ModifierKeys.None)
            {
                if (e.Key == Key.Left)
                {
                    w.Left -= THICK;
                }
                else if (e.Key == Key.Right)
                {
                    w.Left += THICK;

                }
                else if (e.Key == Key.Up)
                {
                    w.Top -= THICK;
                }
                else if (e.Key == Key.Down)
                {
                    w.Top += THICK;
                }
            }
            else
            {
                if (e.Key == Key.Left)
                {
                    w.sldFontSize.Value -= 1;
                }
                else if (e.Key == Key.Right)
                {
                    w.sldFontSize.Value += 1;
                }
                else if (e.Key == Key.Up)
                {
                    Console.WriteLine("[0]" + w.pnlBase.Children[0].ToString());
                    Console.WriteLine("[1]" + w.pnlBase.Children[1].ToString());

                    if (w.txtMessage.MinHeight > 0)
                    {
                        w.txtMessage.MinHeight -= 5;
                    }
                }
                else if (e.Key == Key.Down)
                {
                    if (w.txtMessage.MinHeight + 5 < w.Height)
                    {
                       w.txtMessage.MinHeight += 5;
                    }
                }
            }
            Console.WriteLine("Top:" + w.Top + ", Left:" + w.Left + ", Width:" + w.Width + ", Height:" + w.Height);


        }

    }
}
