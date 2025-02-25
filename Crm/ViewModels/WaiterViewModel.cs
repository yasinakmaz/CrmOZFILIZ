namespace Crm.ViewModels
{
    public class WaiterViewModel : INotifyPropertyChanged
    {
        // Grup isimlendirmesini dilediğiniz gibi değiştirebilirsiniz
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
            if (pastAppointments.Count > 0)
                GroupedItems.Add(pastAppointments);
            if (upcomingAppointments.Count > 0)
                GroupedItems.Add(upcomingAppointments);
            if (assignedRecords.Count > 0)
                GroupedItems.Add(assignedRecords);
            if (myRecords.Count > 0)
                GroupedItems.Add(myRecords);

            OnPropertyChanged(nameof(GroupedItems));
        }
        public ICommand AddOrRemoveGroupDataCommand => new Command<WaiterGroup>((item) =>
        {
            if (item.GroupIcon == "https://img.icons8.com/fluency/96/expand-arrow.png")
            {
                item.Clear();
                item.GroupIcon = "https://img.icons8.com/fluency/96/collapse-arrow.png";
            }
            else
            {
                using var context = new AppDbContext(SqlServices.SqlConnectionString);
                var recordsTobeAdded = context.TBLRECORDLIST
                    .Where(f => f.BusinessAuthNameAndSurname.ToLower().StartsWith(item.GroupTitle.ToLower()))
                    .ToList();

                item.AddRange(recordsTobeAdded);
                item.GroupIcon = "https://img.icons8.com/fluency/96/expand-arrow.png";
            }
        });

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
