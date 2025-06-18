using System.Windows;
using CustomKeyBoard.Enums;

namespace CustomKeyBoard.Controls;

/// <summary>
///     键盘类型附加属性
///     用于控制虚拟键盘的类型
/// </summary>
public class KeyboardTypeProperty
{
    /// <summary>
    ///     键盘类型的依赖属性
    /// </summary>
    public static readonly DependencyProperty TypeProperty =
        DependencyProperty.RegisterAttached(
            "KeyboardType",
            typeof(KeyboardType),
            typeof(KeyboardTypeProperty),
            new PropertyMetadata(KeyboardType.Normal));

    /// <summary>
    ///     获取键盘类型
    /// </summary>
    /// <param name="element">目标元素</param>
    /// <returns>键盘类型</returns>
    public static KeyboardType GetKeyboardType(DependencyObject element)
    {
        return (KeyboardType)element.GetValue(TypeProperty);
    }

    /// <summary>
    ///     设置键盘类型
    /// </summary>
    /// <param name="element">目标元素</param>
    /// <param name="value">键盘类型</param>
    public static void SetKeyboardType(DependencyObject element, KeyboardType value)
    {
        element.SetValue(TypeProperty, value);
    }
}