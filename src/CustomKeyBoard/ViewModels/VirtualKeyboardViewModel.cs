using System.ComponentModel;
using System.Diagnostics;
using CustomKeyBoard.Enums;

namespace CustomKeyBoard.ViewModels;

/// <summary>
///     虚拟键盘的视图模型类
///     负责处理虚拟键盘的业务逻辑和数据绑定
/// </summary>
internal class VirtualKeyboardViewModel : INotifyPropertyChanged
{
    /// <summary>
    ///     标记用户是否点击了确认按钮
    /// </summary>
    private bool _accept = true;

    /// <summary>
    ///     字母键盘模型
    /// </summary>
    private AlphabetModel _alphabet = new();

    /// <summary>
    ///     当前光标位置
    /// </summary>
    private int _caretPosition;

    private bool _isPassword;

    /// <summary>
    ///     当前键盘类型（字母键盘或特殊字符键盘）
    /// </summary>
    private KeyboardPageType _keyboardPageType;

    /// <summary>
    ///     键盘输入的文本内容
    /// </summary>
    private string _keyboardText;

    /// <summary>
    ///     当前选中的文本
    /// </summary>
    private string _selectedValue;

    /// <summary>
    ///     是否为大写模式
    /// </summary>
    private bool _uppercase;

    /// <summary>
    ///     初始化虚拟键盘视图模型
    /// </summary>
    /// <param name="initialValue">初始文本值</param>
    /// <param name="keyboardPageType">类型</param>
    /// <param name="isPassword">是否显示密码模式</param>
    public VirtualKeyboardViewModel(string initialValue, KeyboardPageType keyboardPageType, bool isPassword)
    {
        _keyboardText = initialValue;
        _keyboardPageType = keyboardPageType;
        _isPassword = isPassword;
        _uppercase = false;

        CaretPosition = _keyboardText.Length;

        // 添加字符命令
        AddCharacter = new Command(a =>
        {
            if (a is string character)
                if (character.Length == 1)
                {
                    if (Uppercase) character = character.ToUpper();
                    if (!string.IsNullOrEmpty(SelectedValue))
                    {
                        RemoveSubstring(SelectedValue);
                        KeyboardText = KeyboardText.Insert(CaretPosition, character);
                        CaretPosition++;
                        SelectedValue = "";
                    }
                    else
                    {
                        KeyboardText = KeyboardText.Insert(CaretPosition, character);
                        CaretPosition++;
                    }
                }
        });

        // 切换大小写命令
        ChangeCasing = new Command(a =>
        {
            Uppercase = !Uppercase;
            Alphabet.SetUpperCase(Uppercase);
        });

        // 删除字符命令
        RemoveCharacter = new Command(a =>
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                RemoveSubstring(SelectedValue);
            }
            else
            {
                var position = CaretPosition - 1;
                if (position >= 0)
                {
                    KeyboardText = KeyboardText.Remove(position, 1);
                    if (position < KeyboardText.Length) CaretPosition--;
                    else CaretPosition = KeyboardText.Length;
                }
            }
        });

        // 切换键盘类型命令
        ChangeKeyboardType = new Command(a =>
        {
            if (KeyboardPageType == KeyboardPageType.Alphabet) KeyboardPageType = KeyboardPageType.Special;
            else KeyboardPageType = KeyboardPageType.Alphabet;
        });

        // 确认命令
        Accept = new Command(a =>
        {
            _accept = true;
            CustomKeyboard.Close();
        });

        // 取消命令
        Cancel = new Command(a =>
        {
            _accept = false;
            CustomKeyboard.Close();
        });
    }

    /// <summary>
    ///     获取或设置键盘文本内容
    /// </summary>
    public string KeyboardText
    {
        get => _keyboardText;
        set
        {
            _keyboardText = value;
            NotifyPropertyChanged(nameof(KeyboardText));
        }
    }

    /// <summary>
    ///     获取或设置键盘类型（字母键盘或特殊字符键盘）
    /// </summary>
    public KeyboardPageType KeyboardPageType
    {
        get => _keyboardPageType;
        private set
        {
            _keyboardPageType = value;
            NotifyPropertyChanged(nameof(KeyboardPageType));
        }
    }

    /// <summary>
    ///     获取或设置是否为大写模式
    /// </summary>
    public bool Uppercase
    {
        get => _uppercase;
        private set
        {
            _uppercase = value;
            NotifyPropertyChanged(nameof(Uppercase));
        }
    }

    /// <summary>
    ///     获取或设置光标位置
    /// </summary>
    public int CaretPosition
    {
        get => _caretPosition;
        set
        {
            if (value < 0) _caretPosition = 0;
            else if (value > KeyboardText.Length) _caretPosition = KeyboardText.Length;
            else _caretPosition = value;
            Debug.WriteLine($"CaretPosition {_caretPosition}");

            NotifyPropertyChanged(nameof(CaretPosition));
        }
    }

    /// <summary>
    ///     获取或设置选中的文本
    /// </summary>
    public string SelectedValue
    {
        get => _selectedValue;
        set
        {
            _selectedValue = value;
            NotifyPropertyChanged(nameof(SelectedValue));
        }
    }

    /// <summary>
    ///     获取或设置字母键盘模型
    /// </summary>
    public AlphabetModel Alphabet
    {
        get => _alphabet;
        set
        {
            _alphabet = value;
            NotifyPropertyChanged(nameof(Alphabet));
        }
    }

    /// <summary>
    ///     是否为密码模式
    /// </summary>
    public bool IsPassword
    {
        get => _isPassword;
        set
        {
            _isPassword = value;
            NotifyPropertyChanged(nameof(IsPassword));
        }
    }

    /// <summary>
    ///     添加字符命令
    /// </summary>
    public Command AddCharacter { get; }

    /// <summary>
    ///     切换大小写命令
    /// </summary>
    public Command ChangeCasing { get; }

    /// <summary>
    ///     删除字符命令
    /// </summary>
    public Command RemoveCharacter { get; }

    /// <summary>
    ///     切换键盘类型命令
    /// </summary>
    public Command ChangeKeyboardType { get; }

    /// <summary>
    ///     确认命令
    /// </summary>
    public Command Accept { get; }

    /// <summary>
    ///     取消命令
    /// </summary>
    public Command Cancel { get; }

    /// <summary>
    ///     属性变更事件
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    ///     检查用户是否点击了确认按钮
    /// </summary>
    /// <returns>如果用户点击了确认按钮返回true，否则返回false</returns>
    public bool HasAccept()
    {
        return _accept;
    }

    /// <summary>
    ///     从文本中移除指定的子字符串
    /// </summary>
    /// <param name="substring">要移除的子字符串</param>
    private void RemoveSubstring(string substring)
    {
        var position = KeyboardText.IndexOf(substring);
        KeyboardText = KeyboardText.Remove(position, substring.Length);
    }

    /// <summary>
    ///     通知属性变更
    /// </summary>
    /// <param name="prop">变更的属性名称</param>
    private void NotifyPropertyChanged(string prop)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}