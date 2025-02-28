namespace Crm.Pages;
public partial class BekleyenKayitlarPage : ContentPage
{
    private WaiterListViewModel _viewModel;

    public BekleyenKayitlarPage()
    {
        InitializeComponent();
        _viewModel = new WaiterListViewModel(new AppDbContext(SqlServices.SqlConnectionString));
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await AuthorityControl();
    }

    private async Task AuthorityControl()
    {
        try
        {
            using (var context = new AppDbContext(SqlServices.SqlConnectionString))
            {
                bool giris = await context.TBLPERSONAUTHORITY
                    .Where(a => a.PersonIND == SqlServices.LoginUserGuid)
                    .AnyAsync(a => a.PersonAuthorityID == 1004);

                if (giris)
                {
                    await _viewModel.LoadDataAsync();
                }
                else
                {
                    GrdShow.IsEnabled = false;
                    await Shell.Current.DisplayAlert("Sistem", "Giriþ Ýzniniz Bulunmamaktadýr", "Tamam");
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Hata", $"Sistem hatasý: {ex.Message}", "Tamam");
        }
    }
}