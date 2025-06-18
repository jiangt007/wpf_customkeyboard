using System.Windows;

namespace CustomKeyBoard.Controls;

/// <summary>
///     密码模式显示的附加属性
///     用于控制虚拟键盘输入时的密码显示行为是否为掩码输入
/// </summary>
public class PasswordDisplayProperty
{
    /// <summary>
    ///     是否应将内容视为密码（如掩码输入）
    /// </summary>
    public static readonly DependencyProperty IsPasswordProperty =
        DependencyProperty.RegisterAttached(
            "IsPassword",
            typeof(bool),
            typeof(PasswordDisplayProperty),
            new PropertyMetadata(false));

    public static bool GetIsPassword(DependencyObject element)
    {
        return (bool)element.GetValue(IsPasswordProperty);
    }

    public static void SetIsPassword(DependencyObject element, bool value)
    {
        element.SetValue(IsPasswordProperty, value);
    }
}