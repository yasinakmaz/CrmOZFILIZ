namespace Crm.ViewModels
{
    public class WaiterGroup : ObservableCollection<TblWaiterList>
    {
        public string GroupName { get; set; }
        public bool IsExpanded { get; set; }
        public string ArrowIcon => IsExpanded ? "arrow_up.png" : "arrow_down.png";

        public WaiterGroup(string groupName, bool isExpanded = true)
            : base()
        {
            GroupName = groupName;
            IsExpanded = isExpanded;
        }
    }
}
