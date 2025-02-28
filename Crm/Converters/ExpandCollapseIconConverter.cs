namespace Crm.Converters
{
    public class ExpandCollapseIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isExpanded = (bool)value;
            return isExpanded ? "arrow_down.png" : "arrow_right.png"; // Gerçek resim kaynaklarınızla değiştirin
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
