using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CustomKeyBoard.Controls;

/// <summary>
///     高级文本框控件，继承自TextBox，提供额外的属性和功能
///     主要功能：
///     1. 支持密码掩码显示
///     2. 提供光标位置跟踪
///     3. 支持文本选择状态跟踪
///     4. 与虚拟键盘集成
/// </summary>
internal class AdvancedTextBox : TextBox
{
    /// <summary>
    ///     光标位置的依赖属性
    ///     用于跟踪和更新文本框中的光标位置
    ///     与虚拟键盘的光标位置同步
    /// </summary>
    public static readonly DependencyProperty CaretPositionProperty =
        DependencyProperty.Register("CaretPosition", typeof(int), typeof(AdvancedTextBox),
            new PropertyMetadata(0, OnCaretPositionChange));

    /// <summary>
    ///     选中文本的依赖属性
    ///     用于跟踪和更新文本框中的选中文本内容
    ///     在密码模式下，存储实际选中的文本内容
    /// </summary>
    public static readonly DependencyProperty SelectedValueProperty =
        DependencyProperty.Register("SelectedValue", typeof(string), typeof(AdvancedTextBox),
            new PropertyMetadata("", OnSelectedValueChanged));

    /// <summary>
    ///     文本内容的依赖属性
    ///     用于跟踪和更新文本框中的文本内容
    ///     在密码模式下，存储实际的文本内容
    /// </summary>
    public static readonly DependencyProperty TextValueProperty =
        DependencyProperty.Register("TextValue", typeof(string), typeof(AdvancedTextBox),
            new PropertyMetadata("", OnTextValueChange));

    /// <summary>
    ///     是否启用密码掩码的依赖属性
    ///     控制文本框是否以密码形式显示内容
    ///     默认为false，即普通文本显示模式
    /// </summary>
    public static readonly DependencyProperty IsPasswordProperty =
        DependencyProperty.Register("IsPassword", typeof(bool), typeof(AdvancedTextBox),
            new PropertyMetadata(false, OnIsPasswordChanged));

    /// <summary>
    ///     密码掩码字符的依赖属性
    ///     定义在密码模式下显示的掩码字符
    ///     默认为 ●
    /// </summary>
    public static readonly DependencyProperty PasswordCharProperty =
        DependencyProperty.Register("PasswordChar", typeof(char), typeof(AdvancedTextBox),
            new PropertyMetadata('●', OnPasswordCharChanged));

    /// <summary>
    ///     实际文本内容（用于密码模式）
    ///     在密码模式下存储实际的文本内容
    ///     用于维护文本的实际值，而不是显示值
    /// </summary>
    private string _actualText = string.Empty;

    /// <summary>
    ///     标记控件是否已加载完成
    ///     用于防止在控件初始化过程中触发不必要的事件
    ///     避免在控件加载时触发光标位置更新
    /// </summary>
    private bool _isLoaded;

    /// <summary>
    ///     初始化AdvancedTextBox的新实例
    ///     设置事件处理器和初始状态
    /// </summary>
    public AdvancedTextBox()
    {
        SelectionChanged += AdvancedTextBox_SelectionChanged;
        TextChanged += TextChangedEventHandler;
        Loaded += AdvancedTextBox_Loaded;
    }

    /// <summary>
    ///     获取或设置光标位置
    ///     用于与虚拟键盘同步光标位置
    /// </summary>
    public int CaretPosition
    {
        get => (int)GetValue(CaretPositionProperty);
        set => SetValue(CaretPositionProperty, value);
    }

    /// <summary>
    ///     获取或设置选中的文本值
    ///     在密码模式下，返回实际选中的文本内容
    /// </summary>
    public string SelectedValue
    {
        get => (string)GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    /// <summary>
    ///     获取或设置文本框的文本值
    ///     在密码模式下，存储和返回实际的文本内容
    ///     用于与虚拟键盘进行数据绑定
    /// </summary>
    public string TextValue
    {
        get => (string)GetValue(TextValueProperty);
        set
        {
            Debug.WriteLine($"TextValue SetValue: {value}");
            if (IsPassword)
            {
                _actualText = value;
                Text = new string(PasswordChar, value?.Length ?? 0);
            }
            else
            {
                Text = value;
            }

            SetValue(TextValueProperty, value);
        }
    }

    /// <summary>
    ///     获取或设置是否启用密码掩码
    ///     控制文本框是否以密码形式显示内容
    /// </summary>
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    /// <summary>
    ///     获取或设置密码掩码字符
    ///     定义在密码模式下显示的掩码字符
    /// </summary>
    public char PasswordChar
    {
        get => (char)GetValue(PasswordCharProperty);
        set => SetValue(PasswordCharProperty, value);
    }

    /// <summary>
    ///     文本变更事件处理器
    ///     在密码模式下，使用_actualText更新TextValue
    ///     在非密码模式下，使用Text更新TextValue
    /// </summary>
    private void TextChangedEventHandler(object sender, RoutedEventArgs e)
    {
        if (IsPassword)
        {
            var newValue = _actualText;
            SetValue(TextValueProperty, newValue);
        }
        else
        {
            SetValue(TextValueProperty, Text);
        }
    }

    /// <summary>
    ///     控件加载完成时的事件处理
    ///     标记控件已加载完成，允许触发光标位置更新
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">事件参数</param>
    private void AdvancedTextBox_Loaded(object sender, RoutedEventArgs e)
    {
        _isLoaded = true;
    }

    /// <summary>
    ///     文本选择改变时的事件处理
    ///     更新光标位置和选中文本
    ///     在密码模式下，从_actualText中获取选中的文本
    /// </summary>
    /// <param name="sender">事件源</param>
    /// <param name="e">事件参数</param>
    private void AdvancedTextBox_SelectionChanged(object sender, RoutedEventArgs e)
    {
        Debug.WriteLine(
            $"AdvancedTextBox_SelectionChanged： TextValue = {TextValue} SelectedValue={SelectedValue}，CaretPosition = {CaretIndex}");

        // 在加载完成后才更新 CaretPosition，避免在控件加载时触发不必要的更新
        if (_isLoaded)
            SetValue(CaretPositionProperty, CaretIndex);

        if (IsPassword)
        {
            // 在密码模式下，从_actualText中获取选中的文本
            var startIndex = SelectionStart;
            var length = SelectionLength;
            var selectedText = _actualText.Substring(startIndex, length);
            SetValue(SelectedValueProperty, selectedText);
        }
        else
        {
            SetValue(SelectedValueProperty, SelectedText);
        }
    }

    /// <summary>
    ///     光标位置改变时的回调方法
    ///     更新文本框的光标位置
    /// </summary>
    /// <param name="sender">依赖对象</param>
    /// <param name="e">属性改变事件参数</param>
    private static void OnCaretPositionChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var value = e.NewValue as int?;
        ((TextBox)sender).CaretIndex = value ?? 0;

        Debug.WriteLine($"OnCaretPositionChange: CaretIndex = {value}");
    }

    /// <summary>
    ///     选中文本改变时的回调方法
    ///     在密码模式下，通过更新_actualText来间接更新显示
    ///     在非密码模式下，直接更新SelectedText
    /// </summary>
    /// <param name="sender">依赖对象</param>
    /// <param name="e">属性改变事件参数</param>
    private static void OnSelectedValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var textBox = (AdvancedTextBox)sender;
        var value = e.NewValue as string;

        if (textBox.IsPassword)
        {
            // 在密码模式下，不直接设置SelectedText，而是通过更新_actualText来间接更新显示
            var startIndex = textBox.SelectionStart;
            var length = textBox.SelectionLength;
            if (startIndex >= 0 && length > 0)
            {
                textBox._actualText = textBox._actualText.Remove(startIndex, length);
                if (value != null) textBox._actualText = textBox._actualText.Insert(startIndex, value);
                textBox.Text = new string(textBox.PasswordChar, textBox._actualText.Length);
            }
        }
        else
        {
            textBox.SelectedText = value ?? "";
        }

        Debug.WriteLine($"OnSelectedValueChanged: SelectedText {value}");
    }

    /// <summary>
    ///     密码模式改变时的回调方法
    ///     在切换到密码模式时，将当前文本转换为掩码显示
    ///     在切换到普通模式时，显示实际文本
    /// </summary>
    /// <param name="sender">依赖对象</param>
    /// <param name="e">属性改变事件参数</param>
    private static void OnIsPasswordChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var textBox = (AdvancedTextBox)sender;
        if ((bool)e.NewValue)
        {
            textBox._actualText = textBox.Text;
            textBox.Text = new string(textBox.PasswordChar, textBox._actualText.Length);
        }
        else
        {
            textBox.Text = textBox._actualText;
        }
    }

    /// <summary>
    ///     密码掩码字符改变时的回调方法
    ///     在密码模式下，使用新的掩码字符重新生成显示
    /// </summary>
    /// <param name="sender">依赖对象</param>
    /// <param name="e">属性改变事件参数</param>
    private static void OnPasswordCharChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var textBox = (AdvancedTextBox)sender;
        if (textBox.IsPassword) textBox.Text = new string((char)e.NewValue, textBox._actualText.Length);
    }

    /// <summary>
    ///     文本值改变时的回调方法
    ///     更新文本框的文本内容和光标位置
    ///     在密码模式下，同时更新_actualText和掩码显示
    /// </summary>
    /// <param name="sender">依赖对象</param>
    /// <param name="e">属性改变事件参数</param>
    private static void OnTextValueChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        var s = (AdvancedTextBox)sender;
        var caretPosition = s.CaretPosition;
        var value = e.NewValue as string ?? "";

        if (s.IsPassword)
        {
            s._actualText = value;
            s.Text = new string(s.PasswordChar, value.Length);
        }
        else
        {
            s.Text = value;
        }

        s.CaretIndex = caretPosition <= value.Length ? caretPosition : value.Length;

        Debug.WriteLine($"OnTextValueChange: TextValue = {value}, CaretIndex = {s.CaretIndex}");
    }
}