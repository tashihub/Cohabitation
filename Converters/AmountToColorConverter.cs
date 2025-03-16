using System.Globalization;

namespace Cohabitation.Converters
{
    public class AmountToColorConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var isIncome = ((Label)parameter).Text;
            var amount = (decimal)value;
            if (isIncome == "True")
            {
                return Colors.DarkGreen;
            }
            else
            {
                return Colors.DarkRed;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}