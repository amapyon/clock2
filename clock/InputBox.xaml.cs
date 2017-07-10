using System.Windows;

namespace clock
{
    /// <summary>
    /// InputBox.xaml の相互作用ロジック
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
