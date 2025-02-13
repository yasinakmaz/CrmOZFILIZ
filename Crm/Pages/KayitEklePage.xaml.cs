namespace Crm.Pages;

public partial class KayitEklePage : ContentPage
{
    string sqlservices;
    public KayitEklePage()
    {
        InitializeComponent();
        AuthorityControl();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await SqlServices.InitializeAsync();
        sqlservices = SqlServices.SqlConnectionString;
        LoadData();
    }

    private async void AuthorityControl()
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1001);
            if (giris == true)
            {
            }
            else
            {
                StckLayout.IsEnabled = false;
                await Shell.Current.DisplayAlert("Sistem", "Giriş İzniniz Bulunmamaktadır", "Tamam");
            }
        }
    }

    private async void LoadData()
    {
        try
        {
            if (string.IsNullOrEmpty(sqlservices))
            {
                await Shell.Current.DisplayAlert("Hata", "Bağlantı dizesi yüklenemedi!", "Tamam");
                return;
            }

            List<TblProgram> programs;
            List<TblAgreement> agreements;
            await using (var programContext = new AppDbContext(sqlservices))
            {
                programs = await programContext.TBLPROGRAM.OrderBy(p => p.ProgramName).ToListAsync();
            }
            await using (var agreementContext = new AppDbContext(sqlservices))
            {
                agreements = await agreementContext.TBLAGREEMENT.OrderBy(a => a.AgreementName).ToListAsync();
            }

            ProgramList.ItemsSource = programs ?? new List<TblProgram>();
            AgreementList.ItemsSource = agreements ?? new List<TblAgreement>();
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatası : {ex.Message}", "Tamam");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Genel Hata : {ex.Message}", "Tamam");
        }
    }
    private void RefreshView_Refreshing(object sender, EventArgs e)
    {
        try
        {
            Refresh.IsRefreshing = true;
            LoadData();
        }
        catch (SqlException ex) 
        {
            Refresh.IsRefreshing = false;
            Shell.Current.DisplayAlert("Sistem", $"Sql Hatası : {ex.Message}", "Tamam");
        }
        finally
        {
            Refresh.IsRefreshing = false;
        }
    }

    private void DatePck_CheckChanged(object sender, EventArgs e)
    {
        if (DatePck.IsChecked == true)
        {
            DtpPck.IsVisible = true;
        }
        else
        { DtpPck.IsVisible = false; }
    }
}
