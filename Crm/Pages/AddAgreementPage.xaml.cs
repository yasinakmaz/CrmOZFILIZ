namespace Crm.Pages;

public partial class AddAgreementPage : ContentPage
{
	public AddAgreementPage()
	{
		InitializeComponent();
        AuthorityControl();
    }
    private byte[] _programResimByteArray;
    private string _SpecialCase = "0";
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
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1001);
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

    private void Isbussy(bool ýsbusy)
    {
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
            Isbussy(true);
            await SqlServices.InitializeAsync();
            string sqlservices = SqlServices.SqlConnectionString;

            if (string.IsNullOrWhiteSpace(TxtAgreementName.Text) || _programResimByteArray == null)
            {
                await DisplayAlert("Hata", "Lütfen Tüm Alanlarý Doldurun!", "Tamam");
                return;
            }

            var yeniprogram = new TblAgreement
            {
                IND = Guid.NewGuid(),
                AgreementName = TxtAgreementName.Text,
                SpecialCase = _SpecialCase,
                AgreementImage = _programResimByteArray
            };

            using (var context = new AppDbContext(sqlservices))
            {
                context.TBLAGREEMENT.Add(yeniprogram);
                await context.SaveChangesAsync();
            }

            ClearAll();
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Hata : {ex.Message}","Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        finally
        {
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

                _programResimByteArray = memoryStream.ToArray();
            }
        }
        finally
        {
            Isbussy(false);
        }
    }

    private void ChkSpecialCase_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (ChkSpecialCase.IsChecked == true) { _SpecialCase = "1"; } else { _SpecialCase = "0"; }
    }
}