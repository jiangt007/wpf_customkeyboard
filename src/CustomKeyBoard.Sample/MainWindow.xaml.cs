using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomKeyBoard.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            Label.Content = await CustomKeyboard.OpenAsync();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"密码：{passwordBox?.Password}");
        }

        private void PasswordBox1_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine($"数字密码：{passwordBox1?.Password}");

        }
    }
}