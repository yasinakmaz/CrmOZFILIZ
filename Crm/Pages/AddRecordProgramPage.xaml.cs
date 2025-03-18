namespace Crm.Pages;

public partial class AddRecordProgramPage : ContentPage
{
    public AddRecordProgramPage()
    {
        InitializeComponent();
        AuthorityControl();
        SqlServices.ProgramSelectedItemChanged += OnChange;
    }
    private byte[] _programResimByteArray;
    private string sqlservices;
    private bool change;

    private async void AuthorityControl()
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1007);
            if (giris == true)
            {
            }
            else
            {
                StckLayout.IsEnabled = false;
                await Shell.Current.DisplayAlert("Sistem", "Giriþ Ýzniniz Bulunmamaktadýr", "Tamam");
            }
        }
    }

    private async void OnChange()
    {
        try
        {
            sqlservices = SqlServices.SqlConnectionString;
            TblProgram program;
            await using (var context = new AppDbContext(sqlservices))
            {
                program = await context.TBLPROGRAM.Where(p => p.IND == SqlServices.ProgramUpdateSelectedItem).FirstOrDefaultAsync();
            }

            if (program != null)
            {
                TxtProgramAdi.Text = program.ProgramName;
                TxtProgramCategory.Text = program.ProgramCategory;
                ImgImage.Source = program.ProgramImageSource;
                _programResimByteArray = program.ProgramImage;
                change = true;
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
    }

    private void ClearAll()
    {
        TxtProgramAdi.Text = string.Empty;
        TxtProgramCategory.Text = string.Empty;
        ImgImage.Source = null;
        change = false;
    }

    private void Isbussy(bool ýsbusy)
    {
        BtnShowUpdate.IsEnabled = !ýsbusy;
        ActLoad.IsRunning = ýsbusy;
        TxtProgramAdi.IsEnabled = !ýsbusy;
        TxtProgramCategory.IsEnabled = !ýsbusy;
        BtnPrg.IsEnabled = !ýsbusy;
        BtnClear.IsEnabled = !ýsbusy;
        BtnAdd.IsEnabled = !ýsbusy;
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

                if (string.IsNullOrWhiteSpace(TxtProgramAdi.Text) || _programResimByteArray == null)
                {
                    await DisplayAlert("Hata", "Lütfen tüm alanlarý doldurun!", "Tamam");
                    return;
                }

                var yeniProgram = new TblProgram
                {
                    IND = Guid.NewGuid(),
                    ProgramName = TxtProgramAdi.Text,
                    ProgramCategory = TxtProgramCategory.Text,
                    ProgramImage = _programResimByteArray
                };

                using (var context = new AppDbContext(sqlservices))
                {
                    context.TBLPROGRAM.Add(yeniProgram);
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
                    var mevcutprogram = await context.TBLPROGRAM.SingleOrDefaultAsync(p => p.IND == SqlServices.ProgramUpdateSelectedItem);

                    if (mevcutprogram != null)
                    {
                        mevcutprogram.ProgramName = TxtProgramAdi.Text;
                        mevcutprogram.ProgramCategory = TxtProgramCategory.Text;
                        mevcutprogram.ProgramImage = _programResimByteArray;
                    }

                    await context.SaveChangesAsync();
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

                ImgImage.Source = ImageSource.FromStream(() => new MemoryStream(memoryStream.ToArray()));

                _programResimByteArray = memoryStream.ToArray();
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
        }
        finally
        {
            Isbussy(false);
        }
    }

    private async void BtnShowUpdate_Clicked(object sender, EventArgs e)
    {
        await this.ShowPopupAsync(new ProgramShowUpdate());
    }
}