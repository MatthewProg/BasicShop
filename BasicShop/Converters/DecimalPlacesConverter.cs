using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BasicShop.Converters
{
    public class DecimalPlacesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "0";

            if (parameter.ToString().ToUpper()[0] == 'N')
                return (int)Math.Floor(decimal.Parse(value.ToString()));
            else if (parameter.ToString().ToUpper()[0] == 'D')
            {
                decimal all = decimal.Parse(value.ToString());
                int whole = (int)all;
                var final = all - whole;
                final *= (decimal)Math.Pow(10, double.Parse(parameter.ToString()[1].ToString()));
                var output = ((int)Math.Floor(final)).ToString();
                while(output.Length < int.Parse(parameter.ToString()[1].ToString())) { output += "0"; }
                return output;
            }
            else return "0";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
