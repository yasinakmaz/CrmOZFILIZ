namespace Crm.Pages;

public partial class KayitEklePage : ContentPage
{
    public KayitEklePage()
    {
        InitializeComponent();
        LoadProgramList();
    }

    private void RdbNowDate_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (RdbNowDate.IsChecked == true) { DatePck.IsEnabled = false; DatePck.IsVisible = false; }
        else { DatePck.IsEnabled = true; DatePck.IsVisible = true; }
    }

    private async void LoadProgramList()
    {
        await SqlServices.InitializeAsync();
        string sqlservices = SqlServices.SqlConnectionString;

        using (var context = new AppDbContext(sqlservices))
        {
            var programlar = await context.TBLPROGRAM
                                          .OrderBy(p => p.ProgramAdi)
                                          .ToListAsync();

            ProgramList.ItemsSource = programlar;
        }
    }
}
