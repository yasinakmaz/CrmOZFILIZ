using Microsoft.IdentityModel.Tokens;

namespace Crm.Pages;

public partial class AddUserAuthorityPage : ContentPage
{
	public AddUserAuthorityPage()
	{
		InitializeComponent();
	}

    private async void AuthorityControl()
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1011);
            if (giris == true)
            {
            }
            else
            {
                StckLayout.IsEnabled = false;
                await Shell.Current.DisplayAlert("Sistem", "Giri� �zniniz Bulunmamaktad�r", "Tamam");
            }
        }
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
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

    private void Isbusy(bool �sbusy)
    {
        ActLoad.IsEnabled = �sbusy;
        TxtAuthName.IsEnabled = !�sbusy;
        ChkTeknikKayitAcma.IsEnabled = !�sbusy;
        ChkTeknikAtama.IsEnabled = !�sbusy;
        ChkTeknikDurumAcma.IsEnabled = !�sbusy;
        ChkBekleyenKayitlar.IsEnabled = !�sbusy;
        ChkOnayliKayitlar.IsEnabled = !�sbusy;
        ChkIptalKayitlar.IsEnabled = !�sbusy;
        ChkKullaniciEkle.IsEnabled = !�sbusy;
        ChkKullaniciGuncelle.IsEnabled = !�sbusy;
        ChkYetkiPage.IsEnabled = !�sbusy;
        ChkYetkiUpdate.IsEnabled = !�sbusy;
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
                 { 1007, ChkProgram },
                 { 1008, ChkAgreement },
                 { 1009, ChkKullaniciEkle },
                 { 1010, ChkKullaniciGuncelle },
                 { 1011, ChkYetkiPage },
                 { 1012, ChkYetkiUpdate }
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
                                PersonIND = SqlServices.CreatUserGuid,
                                PersonAuthorityID = kvp.Key
                            };

                            context.TBLPERSONAUTHORITY.Add(tblauth);
                        }
                    }

                    await context.SaveChangesAsync();
                    await Shell.Current.GoToAsync($"///{nameof(AddPersonPage)}");
                }
            }
            catch (SqlException ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Sql Hatas� : {ex.Message}", "Tamam");
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
            await Shell.Current.DisplayAlert("Sistem", "Yetki Ad� Bo� B�rak�lamaz", "Tamam");
        }
    }
}