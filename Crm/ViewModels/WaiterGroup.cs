namespace Crm.ViewModels
{
    public class WaiterGroup : ObservableRangeCollection<TblWaiterList>, INotifyPropertyChanged
    {
        public string GroupTitle { get; set; }
        public string FooterTitle { get; set; }

        private string _groupIcon = "https://img.icons8.com/fluency/96/expand-arrow.png";
        public string GroupIcon
        {
            get => _groupIcon;
            set => SetProperty(ref _groupIcon, value);
        }

        private bool _isExpanded;
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public WaiterGroup(string groupTitle, bool isExpanded = true, string footerTitle = "")
        {
            GroupTitle = groupTitle;
            FooterTitle = footerTitle;
            IsExpanded = isExpanded;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "", Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}