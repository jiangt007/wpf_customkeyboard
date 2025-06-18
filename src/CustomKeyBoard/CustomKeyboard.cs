using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using CustomKeyBoard.Controls;
using CustomKeyBoard.Enums;
using CustomKeyBoard.ViewModels;
using CustomKeyBoard.Views;

namespace CustomKeyBoard;

/// <summary>
///     虚拟键盘管理类
///     提供虚拟键盘的配置、监听、打开和关闭等功能
/// </summary>
public class CustomKeyboard
{
    /// <summary>
    ///     键盘值内容控件的名称
    /// </summary>
    private const string KeyboardValueName = "KeyboardValueContent";

    /// <summary>
    ///     键盘内容控件的名称
    /// </summary>
    private const string KeyboardName = "KeyboardContent";

    /// <summary>
    ///     键盘窗口宿主类型
    /// </summary>
    private static Type _hostType = typeof(DefaultKeyboardHost);

    /// <summary>
    ///     用于异步等待键盘输入结果的任务完成源
    /// </summary>
    private static TaskCompletionSource<string>? _tcs;

    /// <summary>
    ///     当前打开的键盘窗口实例
    /// </summary>
    private static Window? _windowHost;

    /// <summary>
    ///     配置虚拟键盘的窗口宿主类型
    /// </summary>
    /// <param name="hostType">必须是Window的子类</param>
    /// <exception cref="ArgumentException">当hostType不是Window的子类时抛出</exception>
    public static void Config(Type hostType)
    {
        if (hostType.IsSubclassOf(typeof(Window))) _hostType = hostType;
        else throw new ArgumentException();
    }

    /// <summary>
    ///     为指定UI元素的字符串属性添加虚拟键盘监听
    /// </summary>
    /// <typeparam name="T">UI元素类型</typeparam>
    /// <param name="property">要监听的字符串属性表达式</param>
    public static void Listen<T>(Expression<Func<T, string>> property) where T : UIElement
    {
        EventManager.RegisterClassHandler(typeof(T), UIElement.PreviewMouseLeftButtonDownEvent,
            (RoutedEventHandler)(async (s, e) =>
            {
                if (s is AdvancedTextBox) return;
                var memberSelectorExpression = property.Body as MemberExpression;
                if (memberSelectorExpression == null) return;
                var prop = memberSelectorExpression.Member as PropertyInfo;
                if (prop == null) return;
                var initValue = (string)prop.GetValue(s);
                var keyboardType = KeyboardTypeProperty.GetKeyboardType((DependencyObject)s);
                var passwordDisplay = PasswordDisplayProperty.GetIsPassword((DependencyObject)s);
                var value = await OpenAsync(initValue, keyboardType, passwordDisplay);
                prop.SetValue(s, value, null);
                UpdateCaretIndex<T>(s, value);
            }));
    }

    /// <summary>
    ///     更新指定UI元素的光标位置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="s"></param>
    /// <param name="value"></param>
    private static void UpdateCaretIndex<T>(object s, string value) where T : UIElement
    {
        if (s is TextBox textBox)
            textBox.CaretIndex = value.Length;
        else if (s is PasswordBox passwordBox) SetPasswordBoxCaretToEnd(passwordBox);
    }

    /// <summary>
    ///     设置 PasswordBox 的光标位置到末尾
    /// </summary>
    /// <param name="passwordBox"></param>
    private static void SetPasswordBoxCaretToEnd(PasswordBox passwordBox)
    {
        if (passwordBox == null) return;

        passwordBox.Focus();

        // 反射调用 PasswordBox 的 Select 方法（非公开）
        var method = typeof(PasswordBox).GetMethod("Select", BindingFlags.Instance | BindingFlags.NonPublic);
        if (method != null)
        {
            var length = passwordBox.Password.Length;
            method.Invoke(passwordBox, new object[] { length, 0 }); // 光标移到末尾
        }
    }

    /// <summary>
    ///     异步打开虚拟键盘
    /// </summary>
    /// <param name="initialValue">初始文本值</param>
    /// <param name="keyboardType"></param>
    /// <returns>包含用户输入结果的Task</returns>
    /// <exception cref="InvalidOperationException">当已有键盘窗口打开时抛出</exception>
    public static Task<string> OpenAsync(string initialValue = "", KeyboardType keyboardType = KeyboardType.Normal,
        bool isPassword = false)
    {
        if (_windowHost != null) throw new InvalidOperationException();

        _tcs = new TaskCompletionSource<string>();
        _windowHost = (Window)Activator.CreateInstance(_hostType);
        _windowHost.DataContext = new VirtualKeyboardViewModel(initialValue, KeyboardPageType.Alphabet, isPassword);
        ((ContentControl)_windowHost.FindName(KeyboardValueName)).Content = new KeyboardValueView();
        ((ContentControl)_windowHost.FindName(KeyboardName)).Content = GetKeyboardView(keyboardType);

        void handler(object s, CancelEventArgs a)
        {
            var result = GetResult();
            var accept = Accept();
            ((Window)s).Closing -= handler;
            _windowHost = null;
            if (accept)
                _tcs?.SetResult(result);
            _tcs = null;
        }

        _windowHost.Closing += handler;

        _windowHost.Owner = Application.Current.MainWindow;
        _windowHost.Show();
        return _tcs.Task;
    }

    /// <summary>
    ///     关闭虚拟键盘窗口
    /// </summary>
    /// <exception cref="InvalidOperationException">当没有打开的键盘窗口时抛出</exception>
    public static void Close()
    {
        if (_windowHost == null) throw new InvalidOperationException();

        _windowHost.Close();
    }

    /// <summary>
    ///     获取当前键盘输入的文本结果
    /// </summary>
    /// <returns>键盘输入的文本</returns>
    private static string GetResult()
    {
        var viewModel = (VirtualKeyboardViewModel)_windowHost.DataContext;
        return viewModel.KeyboardText;
    }

    /// <summary>
    ///     检查用户是否点击了确认按钮
    /// </summary>
    /// <returns>如果用户点击了确认按钮返回true，否则返回false</returns>
    private static bool Accept()
    {
        var viewModel = (VirtualKeyboardViewModel)_windowHost.DataContext;
        return viewModel.HasAccept();
    }

    /// <summary>
    ///     根据键盘类型获取对应的键盘视图
    /// </summary>
    /// <param name="keyboardPageType">类型</param>
    /// <returns>键盘视图</returns>
    private static UserControl GetKeyboardView(KeyboardType keyboardPageType)
    {
        return keyboardPageType switch
        {
            KeyboardType.Numeric => new NumericKeyboardView(),
            KeyboardType.Normal => new VirtualKeyboardView(),
            _ => new VirtualKeyboardView()
        };
    }
}