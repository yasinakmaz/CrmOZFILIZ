namespace Crm.Pages;

public partial class AgreementShowUpdate : Popup
{
    private string sqlservices;
    private TblAgreement selectedAgreement;
    public AgreementShowUpdate()
    {
        InitializeComponent();
        OnLoad();
    }

    private async void OnLoad()
    {
        await SqlServices.InitializeAsync();
        sqlservices = SqlServices.SqlConnectionString;
        LoadData();
    }

    private async void LoadData()
    {
        try
        {
            if (string.IsNullOrEmpty(sqlservices))
            {
                await Shell.Current.DisplayAlert("Hata", "Baðlantý dizesi yüklenemedi!", "Tamam");
                return;
            }

            List<TblAgreement> agreements;
            await using (var agreementcontext = new AppDbContext(sqlservices))
            {
                agreements = await agreementcontext.TBLAGREEMENT.OrderBy(p => p.AgreementName).ToListAsync();
            }

            LstView.ItemsSource = agreements ?? new List<TblAgreement>();
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Genel Hata : {ex.Message}", "Tamam");
        }
    }

    private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            if (TxtSearch.Text.Length >= 3)
            {
                List<TblAgreement> filteredagreement;
                await using (var agreementcontext = new AppDbContext(sqlservices))
                {
                    filteredagreement = await agreementcontext.TBLAGREEMENT.Where(p => p.AgreementName.Contains(TxtSearch.Text)).ToListAsync();
                }
                LstView.ItemsSource = filteredagreement ?? new List<TblAgreement>();
            }
            else if (TxtSearch.Text.Length == 0)
            {
                LoadData();
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
        }
    }

    private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is TblAgreement agreement)
        {
            selectedAgreement = agreement;
        }
    }

    private async void BtnChange_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (selectedAgreement == null)
            {
                await Shell.Current.DisplayAlert("Sistem", "Lütfen Düzenlemek Ýçin Program Seçiniz", "Tamam");
                return;
            }
            else
            {
                SqlServices.ProgramUpdateSelectedItem = selectedAgreement.IND;
                SqlServices.ChangeProgramSelectedItemChanged();
                await this.CloseAsync();
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
        }
    }

    private async void BtnDelete_Clicked(object sender, EventArgs e)
    {
        if (selectedAgreement == null)
        {
            await Shell.Current.DisplayAlert("Sistem", "Lütfen Düzenlemek Ýçin Program Seçiniz", "Tamam");
            return;
        }
        try
        {
            await using (var context = new AppDbContext(sqlservices))
            {
                var agreementToDelete = await context.TBLAGREEMENT.FindAsync(selectedAgreement.IND);
                if (agreementToDelete != null)
                {
                    context.TBLAGREEMENT.Remove(agreementToDelete);
                    await context.SaveChangesAsync();
                    LoadData();
                }
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
        }
    }
}