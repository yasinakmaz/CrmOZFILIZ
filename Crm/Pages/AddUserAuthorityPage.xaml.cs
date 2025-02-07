using Microsoft.IdentityModel.Tokens;

namespace Crm.Pages;

public partial class AddUserAuthorityPage : ContentPage
{
    private Guid guid;
	public AddUserAuthorityPage(Guid IND)
	{
		InitializeComponent();
        this.guid = IND;
	}

    private void ClearAll()
    {
        TxtAuthName.Text = string.Empty;
        ChkTeknikKayitAcma.IsChecked = false;
        ChkTeknikAtama.IsChecked = false;
        ChkTeknikDurumAcma.IsChecked = false;
        ChkBekleyenKayitlar.IsChecked = false;
        ChkOnayliKayitlar.IsChecked = false;
        ChkIptalKayitlar.IsChecked = false;
        ChkKullaniciEkle.IsChecked = false;
        ChkKullaniciGuncelle.IsChecked = false;
        ChkYetkiPage.IsChecked = false;
        ChkYetkiUpdate.IsChecked = false;
    }

    private void Isbusy(bool ýsbusy)
    {
        ActLoad.IsEnabled = ýsbusy;
        TxtAuthName.IsEnabled = !ýsbusy;
        ChkTeknikKayitAcma.IsEnabled = !ýsbusy;
        ChkTeknikAtama.IsEnabled = !ýsbusy;
        ChkTeknikDurumAcma.IsEnabled = !ýsbusy;
        ChkBekleyenKayitlar.IsEnabled = !ýsbusy;
        ChkOnayliKayitlar.IsEnabled = !ýsbusy;
        ChkIptalKayitlar.IsEnabled = !ýsbusy;
        ChkKullaniciEkle.IsEnabled = !ýsbusy;
        ChkKullaniciGuncelle.IsEnabled = !ýsbusy;
        ChkYetkiPage.IsEnabled = !ýsbusy;
        ChkYetkiUpdate.IsEnabled = !ýsbusy;
    }

    private void BtnClear_Clicked(object sender, EventArgs e)
    {
        ClearAll();
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        if (!TxtAuthName.Text.IsNullOrEmpty())
        {
            try
            {
                Isbusy(true);
                await SqlServices.InitializeAsync();
                string sqlservices = SqlServices.SqlConnectionString;

                var authorityMapping = new Dictionary<int, CheckBox>
             {
                 { 1001, ChkTeknikKayitAcma },
                 { 1002, ChkTeknikAtama },
                 { 1003, ChkTeknikDurumAcma },
                 { 1004, ChkBekleyenKayitlar },
                 { 1005, ChkOnayliKayitlar },
                 { 1006, ChkIptalKayitlar },
                 { 1007, ChkKullaniciEkle },
                 { 1008, ChkKullaniciGuncelle },
                 { 1009, ChkYetkiPage },
                 { 1010, ChkYetkiUpdate }
             };

                using (var context = new AppDbContext(sqlservices))
                {
                    foreach (var kvp in authorityMapping)
                    {
                        if (kvp.Value.IsChecked)
                        {
                            var tblauth = new TblPersonAuthority
                            {
                                IND = Guid.NewGuid(),
                                AuhtorityName = TxtAuthName.Text,
                                PersonIND = guid,
                                PersonAuthorityID = kvp.Key
                            };

                            context.TBLPERSONAUTHORITY.Add(tblauth);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (SqlException ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
                await Clipboard.SetTextAsync(ex.Message);
            }
            finally
            {
                ClearAll();
                Isbusy(false);
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Sistem", "Yetki Adý Boþ Býrakýlamaz", "Tamam");
        }
    }
}