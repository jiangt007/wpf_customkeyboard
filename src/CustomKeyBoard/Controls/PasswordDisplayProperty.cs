using System.Windows;

namespace CustomKeyBoard.Controls;

/// <summary>
///     ����ģʽ��ʾ�ĸ�������
///     ���ڿ��������������ʱ��������ʾ��Ϊ�Ƿ�Ϊ��������
/// </summary>
public class PasswordDisplayProperty
{
    /// <summary>
    ///     �Ƿ�Ӧ��������Ϊ���루���������룩
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