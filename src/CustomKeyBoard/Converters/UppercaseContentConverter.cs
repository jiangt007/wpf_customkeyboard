using System.Globalization;
using System.Windows.Data;

namespace CustomKeyBoard.Converters;

internal class UppercaseContentConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string content)
        {
            var uppercase = (bool)(parameter ?? false);
            return uppercase ? content.ToUpper() : content.ToLower();
        }

        return value;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}