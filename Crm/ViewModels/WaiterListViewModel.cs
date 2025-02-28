namespace Crm.ViewModels
{
    public class WaiterListViewModel : BindableObject
    {
        private ObservableCollection<StatusGroup> _groupedWaiters;
        public ObservableCollection<StatusGroup> GroupedWaiters
        {
            get => _groupedWaiters;
            set
            {
                _groupedWaiters = value;
                OnPropertyChanged();
            }
        }

        private ICommand _toggleGroupCommand;
        public ICommand ToggleGroupCommand => _toggleGroupCommand ??= new Command<StatusGroup>(ToggleGroup);

        private readonly AppDbContext _dbContext;

        public WaiterListViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            GroupedWaiters = new ObservableCollection<StatusGroup>();
            LoadGroupedDataAsync();
        }

        private void ToggleGroup(StatusGroup group)
        {
            if (group == null) return;

            group.IsExpanded = !group.IsExpanded;

            // Force refresh the CollectionView to update item visibility
            var temp = new ObservableCollection<StatusGroup>(GroupedWaiters);
            GroupedWaiters = null;
            GroupedWaiters = temp;
        }

        public async void LoadGroupedDataAsync()
        {
            try
            {
                await LoadGroupedData();
            }
            catch (Exception ex)
            {
                // Hata yönetimi burada
                Console.WriteLine($"Veri yükleme hatası: {ex.Message}");
            }
        }

        public async Task LoadGroupedData()
        {
            // Get all statuses ordered by Order
            var statuses = await _dbContext.TBLSTATUS
                .OrderBy(s => s.Order)
                .ToListAsync();

            // Get all waiter list items
            var waiters = await _dbContext.TBLRECORDLIST.ToListAsync();

            // Create groups
            var groups = new ObservableCollection<StatusGroup>();

            foreach (var status in statuses)
            {
                var itemsInStatus = waiters
                    .Where(w => w.RecordStatusInd == status.IND)
                    .ToList();

                if (itemsInStatus.Any())
                {
                    groups.Add(new StatusGroup(status, itemsInStatus));
                }
            }

            GroupedWaiters = groups;
        }
    }

    // Group class for CollectionView grouping
    public class StatusGroup : ObservableCollection<TblWaiterList>
    {
        public TblStatus Status { get; }
        public bool IsExpanded { get; set; } = true;

        public StatusGroup(TblStatus status, IEnumerable<TblWaiterList> waiters) : base(waiters)
        {
            Status = status;
        }
    }
}
