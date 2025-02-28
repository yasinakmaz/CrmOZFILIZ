namespace Crm.ViewModels
{
    public class WaiterListViewModel : BindableObject
    {
        private ObservableCollection<GroupModel> _groups;
        public ObservableCollection<GroupModel> Groups
        {
            get => _groups;
            set
            {
                _groups = value;
                OnPropertyChanged();
            }
        }

        private readonly AppDbContext _dbContext;

        public WaiterListViewModel()
        {
            Groups = new ObservableCollection<GroupModel>();
        }

        public WaiterListViewModel(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Groups = new ObservableCollection<GroupModel>();
        }

        public async Task LoadDataAsync()
        {
            if (_dbContext == null)
            {
                System.Diagnostics.Debug.WriteLine("DbContext null - veri yüklenemedi");
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine("Veri yükleniyor...");

                Groups.Clear();

                var statuses = await _dbContext.TBLSTATUS.OrderBy(s => s.Order).ToListAsync();
                var waiters = await _dbContext.TBLRECORDLIST.ToListAsync();

                System.Diagnostics.Debug.WriteLine($"Yüklenen durum sayısı: {statuses.Count}, Bekleyen kayıt sayısı: {waiters.Count}");

                foreach (var status in statuses)
                {
                    var items = waiters.Where(w => w.RecordStatusInd == status.IND).ToList();
                    if (items.Any())
                    {
                        var group = new GroupModel(status);
                        foreach (var item in items)
                        {
                            group.Items.Add(item);
                        }
                        Groups.Add(group);
                        System.Diagnostics.Debug.WriteLine($"Grup eklendi: {status.TitleText}, Öğe sayısı: {items.Count}");
                    }
                }
                OnPropertyChanged(nameof(Groups));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Veri yükleme hatası: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }

    public class GroupModel : BindableObject
    {
        public TblStatus Status { get; }

        private ObservableCollection<TblWaiterList> _items = new ObservableCollection<TblWaiterList>();
        public ObservableCollection<TblWaiterList> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        private bool _isExpanded = true;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded != value)
                {
                    _isExpanded = value;
                    OnPropertyChanged();
                    System.Diagnostics.Debug.WriteLine($"Grup genişletme/daraltma: {Status?.TitleText}, Değer: {_isExpanded}");
                }
            }
        }

        public ICommand ToggleCommand { get; }

        public GroupModel(TblStatus status)
        {
            Status = status;
            ToggleCommand = new Command(Toggle);
        }

        private void Toggle()
        {
            IsExpanded = !IsExpanded;
        }
    }
}