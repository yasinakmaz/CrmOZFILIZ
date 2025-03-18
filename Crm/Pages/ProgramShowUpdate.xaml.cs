namespace Crm.Pages;

public partial class ProgramShowUpdate : Popup
{
    private string sqlservices;
    private TblProgram selectedProgram;
    public ProgramShowUpdate()
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

            List<TblProgram> programs;
            await using (var programContext = new AppDbContext(sqlservices))
            {
                programs = await programContext.TBLPROGRAM.OrderBy(p => p.ProgramName).ToListAsync();
            }

            LstView.ItemsSource = programs ?? new List<TblProgram>();
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
                List<TblProgram> filteredprograms;
                await using (var programcontext = new AppDbContext(sqlservices))
                {
                    filteredprograms = await programcontext.TBLPROGRAM.Where(p => p.ProgramName.Contains(TxtSearch.Text)).ToListAsync();
                }
                LstView.ItemsSource = filteredprograms ?? new List<TblProgram>();
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
        if (e.SelectedItem is TblProgram program)
        {
            selectedProgram = program;
        }
    }

    private async void BtnChange_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (selectedProgram == null)
            {
                await Shell.Current.DisplayAlert("Sistem", "Lütfen Düzenlemek Ýçin Program Seçiniz", "Tamam");
                return;
            }
            else
            {
                SqlServices.ProgramUpdateSelectedItem = selectedProgram.IND;
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
        if (selectedProgram == null)
        {
            await Shell.Current.DisplayAlert("Sistem", "Lütfen Düzenlemek Ýçin Program Seçiniz", "Tamam");
            return;
        }
        try
        {
            await using (var context = new AppDbContext(sqlservices))
            {
                var programToDelete = await context.TBLPROGRAM.FindAsync(selectedProgram.IND);
                if (programToDelete != null)
                {
                    context.TBLPROGRAM.Remove(programToDelete);
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