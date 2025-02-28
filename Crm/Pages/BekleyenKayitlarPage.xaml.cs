namespace Crm.Pages;

public partial class BekleyenKayitlarPage : ContentPage
{
    public BekleyenKayitlarPage()
	{
		InitializeComponent();
        BindingContext = App.Current.Handler.MauiContext.Services.GetService<WaiterListViewModel>();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        AuthorityControl();
    }
    private async void AuthorityControl()
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1004);
            if (giris == true)
            {
                if (BindingContext is WaiterListViewModel viewModel)
                {
                    await viewModel.LoadGroupedData();
                }
            }
            else
            {
                GrdShow.IsEnabled = false;
                await Shell.Current.DisplayAlert("Sistem", "Giriþ Ýzniniz Bulunmamaktadýr", "Tamam");
            }
        }
    }
}