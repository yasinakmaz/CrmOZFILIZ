namespace Crm.Pages;

public partial class AddAgreementPage : ContentPage
{
    public AddAgreementPage()
    {
        InitializeComponent();
        AuthorityControl();
        SqlServices.ProgramSelectedItemChanged += OnChange;
    }
    private byte[] _agreementResimByteArray;
    private string _SpecialCase = "0";
    private string sqlservices;
    private bool change;
    private void ClearAll()
    {
        TxtAgreementName.Text = string.Empty;
        ImageShow.Source = null;
        ChkSpecialCase.IsChecked = false;
    }

    private async void AuthorityControl()
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1008);
            if (giris == true)
            {
            }
            else
            {
                GrdShow.IsEnabled = false;
                await Shell.Current.DisplayAlert("Sistem", "Giriþ Ýzniniz Bulunmamaktadýr", "Tamam");
            }
        }
    }

    private async void OnChange()
    {
        try
        {
            sqlservices = SqlServices.SqlConnectionString;
            TblAgreement agreement;
            await using (var context = new AppDbContext(sqlservices))
            {
                agreement = await context.TBLAGREEMENT.Where(p => p.IND == SqlServices.ProgramUpdateSelectedItem).FirstOrDefaultAsync();
            }

            if (agreement != null)
            {
                TxtAgreementName.Text = agreement.AgreementName;
                ImageShow.Source = agreement.AgreementImageSource;
                _agreementResimByteArray = agreement.AgreementImage;
                _SpecialCase = agreement.SpecialCase;
                if (_SpecialCase == "1")
                {
                    ChkSpecialCase.IsChecked = true;
                }
                else
                {
                    ChkSpecialCase.IsChecked = false;
                }
                change = true;
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
    }

    private void Isbussy(bool ýsbusy)
    {
        BtnShowUpdate.IsEnabled = !ýsbusy;
        ActLoad.IsRunning = ýsbusy;
        TxtAgreementName.IsEnabled = !ýsbusy;
        BtnImagePicker.IsEnabled = !ýsbusy;
        BtnClear.IsEnabled = !ýsbusy;
        BtnAdd.IsEnabled = !ýsbusy;
        ChkSpecialCase.IsEnabled = !ýsbusy;
    }

    private void BtnClear_Clicked(object sender, EventArgs e)
    {
        ClearAll();
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (change == false)
            {
                Isbussy(true);
                await SqlServices.InitializeAsync();
                string sqlservices = SqlServices.SqlConnectionString;

                if (string.IsNullOrWhiteSpace(TxtAgreementName.Text) || _agreementResimByteArray == null)
                {
                    await DisplayAlert("Hata", "Lütfen Tüm Alanlarý Doldurun!", "Tamam");
                    return;
                }

                var yeniagreement = new TblAgreement
                {
                    IND = Guid.NewGuid(),
                    AgreementName = TxtAgreementName.Text,
                    SpecialCase = _SpecialCase,
                    AgreementImage = _agreementResimByteArray
                };

                using (var context = new AppDbContext(sqlservices))
                {
                    context.TBLAGREEMENT.Add(yeniagreement);
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                Isbussy(true);
                await SqlServices.InitializeAsync();
                string sqlservices = SqlServices.SqlConnectionString;

                using (var context = new AppDbContext(sqlservices))
                {
                    var mevcutagreement = await context.TBLAGREEMENT.SingleOrDefaultAsync(p => p.IND == SqlServices.ProgramUpdateSelectedItem);

                    if (mevcutagreement != null)
                    {
                        mevcutagreement.AgreementName = TxtAgreementName.Text;
                        mevcutagreement.SpecialCase = _SpecialCase;
                        mevcutagreement.AgreementImage = _agreementResimByteArray;
                    }

                    await context.SaveChangesAsync();
                }
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Hata : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        finally
        {
            ClearAll();
            Isbussy(false);
        }
    }

    private async void BtnPrg_Clicked(object sender, EventArgs e)
    {
        try
        {
            Isbussy(true);
            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                using var stream = await result.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);

                ImageShow.Source = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));

                _agreementResimByteArray = memoryStream.ToArray();
            }
        }
        finally
        {
            Isbussy(false);
        }
    }

    private async void BtnShowUpdate_Clicked(object sender, EventArgs e)
    {
        await this.ShowPopupAsync(new AgreementShowUpdate());
    }

    private void ChkSpecialCase_CheckChanged(object sender, EventArgs e)
    {
        if (ChkSpecialCase.IsChecked == true) { _SpecialCase = "1"; } else { _SpecialCase = "0"; }
    }
}