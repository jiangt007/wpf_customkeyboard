using System.Globalization;
using System.Windows.Data;
using CustomKeyBoard.Enums;
using CustomKeyBoard.Views;

namespace CustomKeyBoard.Converters;

internal class KeyboardTypeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        var type = (KeyboardPageType)(value ?? KeyboardPageType.Alphabet);
        switch (type)
        {
            case KeyboardPageType.Alphabet: return new AlphabetView();
            case KeyboardPageType.Special: return new SpecialCharactersView();
        }

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}