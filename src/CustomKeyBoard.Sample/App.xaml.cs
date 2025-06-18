using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CustomKeyBoard.Sample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            CustomKeyboard.Listen<TextBox>(e => e.Text);
            CustomKeyboard.Listen<PasswordBox>(e => e.Password);
        }
    }
}