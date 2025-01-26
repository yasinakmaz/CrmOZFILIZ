using Crm.Model;
using System.Collections.ObjectModel;

namespace Crm.Pages;

public partial class BekleyenKayitlarPage : ContentPage
{
    public ObservableCollection<TblPortalModel> Kayitlar { get; set; }
    public BekleyenKayitlarPage()
	{
		InitializeComponent();
        Kayitlar = new ObservableCollection<TblPortalModel>();
        KayitCollectionView.ItemsSource = Kayitlar;
        LoadData();
    }
    private async void LoadData()
    {
        SqlConnection conn = new SqlConnection(SqlServices.SqlConnectionString);

        try
        {
            await conn.OpenAsync();
            string query = "SELECT ID, Secim, AdSoyad, Firma, TelNo, Sorun, Resim FROM TblPortalBekleyenKayitlar";
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                byte[]? imageBytes = null;

                if (!reader.IsDBNull(6))
                {
                    imageBytes = (byte[])reader["Resim"];
                }

                Kayitlar.Add(new TblPortalModel
                {
                    ID = reader.GetInt32(0),
                    Secim = reader.GetString(1),
                    AdSoyad = reader.GetString(2),
                    Firma = reader.GetString(3),
                    TelNo = reader.GetString(4),
                    Sorun = reader.GetString(5),
                    Resim = imageBytes
                });
            }

            await reader.CloseAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Veriler yüklenirken bir hata oluþtu: " + ex.Message, "Tamam");
        }
        finally
        {
            await conn.CloseAsync();
        }
    }
}