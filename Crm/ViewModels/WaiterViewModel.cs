using System.Windows.Input;

namespace Crm.ViewModels
{
    public class WaiterViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WaiterGroup> GroupedItems { get; set; } = new();
        private readonly Guid _loginUserGuid = SqlServices.LoginUserGuid;

        public WaiterViewModel()
        {
            LoadData();
        }

        private async void LoadData()
        {
            using var context = new AppDbContext(SqlServices.SqlConnectionString);
            var allItems = await context.TBLRECORDLIST.ToListAsync();

            var pastAppointments = new WaiterGroup("Geçmiş Ama Kapanmayan Randevularım", true);
            var upcomingAppointments = new WaiterGroup("Yaklaşan Randevular", true);
            var assignedRecords = new WaiterGroup("Atanandığım Kayıtlar", true);
            var myRecords = new WaiterGroup("Kayıtlarım", true);

            foreach (var item in allItems)
            {
                if (item.Meeting && item.MeetingDate < DateTime.Now)
                    pastAppointments.Add(item);
                else if (item.Meeting && item.MeetingDate >= DateTime.Now && item.MeetingDate <= DateTime.Now.AddDays(15))
                    upcomingAppointments.Add(item);
                else if (item.MomentPersonInd == _loginUserGuid)
                    assignedRecords.Add(item);
                else if (item.AddRecordPersonInd == _loginUserGuid)
                    myRecords.Add(item);
            }

            GroupedItems.Clear();
            GroupedItems.Add(pastAppointments);
            GroupedItems.Add(upcomingAppointments);
            GroupedItems.Add(assignedRecords);
            GroupedItems.Add(myRecords);

            OnPropertyChanged(nameof(GroupedItems));
        }

        public void ToggleGroup(WaiterGroup group)
        {
            group.IsExpanded = !group.IsExpanded;
            OnPropertyChanged(nameof(GroupedItems));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICommand ToggleGroupCommand => new Command<WaiterGroup>((group) =>
        {
            group.IsExpanded = !group.IsExpanded;
            OnPropertyChanged(nameof(GroupedItems));
        });
    }
}
