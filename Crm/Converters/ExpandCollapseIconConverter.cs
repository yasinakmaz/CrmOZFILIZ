namespace Crm.Converters
{
    public class ExpandCollapseIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            System.Diagnostics.Debug.WriteLine($"ExpandCollapseIconConverter: {value}");

            bool isExpanded = (bool)value;
            return isExpanded ? "down.png" : "up.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}