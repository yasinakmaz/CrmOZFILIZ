using Microsoft.IdentityModel.Tokens;

namespace Crm.Pages;

public partial class KayitEklePage : ContentPage
{
    public KayitEklePage()
    {
        InitializeComponent();
        LoadProgramList();
    }
    private async void LoadProgramList()
    {
        try
        {
            await SqlServices.InitializeAsync();
            string sqlservices = SqlServices.SqlConnectionString;

            using (var context = new AppDbContext(sqlservices))
            {
                var programlar = await context.TBLPROGRAM
                                              .OrderBy(p => p.ProgramName)
                                              .ToListAsync();

                ProgramList.ItemsSource = programlar ?? new List<TblProgram>();
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"SQL Hatası: {ex.Message}");
            ProgramList.ItemsSource = new List<TblProgram>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Genel Hata: {ex.Message}");
            ProgramList.ItemsSource = new List<TblProgram>();
        }
    }

    private async void RdbAnlasmaozeldurum_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if(RdbAnlasmaozeldurum.IsChecked == true)
            {
                string result = await DisplayPromptAsync("Sistem", "Özel Durum Nedininizi Belirtiniz !*");
                if (string.IsNullOrWhiteSpace(result)) 
                { 
                    RdbAnlasmaozeldurum.IsChecked = false; 
                    await Toast.Make("Ekrana dokunmadınız!", ToastDuration.Long).Show(); 
                } 
                else {}
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sistem", $"Hata : {ex.Message}", "Tamam");
        }
        finally
        {
        }
    }
}
