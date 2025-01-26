namespace Crm.Services
{
    public class SecimToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string secim)
            {
                return secim switch
                {
                    "High" => "https://img.icons8.com/arcade/64/high-priority.png",
                    "Med" => "https://img.icons8.com/arcade/64/medium-priority.png",
                    "Low" => "https://img.icons8.com/arcade/64/low-risk.png"
                };
            }

            return "https://img.icons8.com/arcade/64/low-risk.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
