namespace Crm.Pages;

public partial class AddPersonPage : ContentPage
{
    private byte[] _addpersonImage;
    private bool _addperson;
    public AddPersonPage()
	{
		InitializeComponent();
        AuthorityControl();
        TxtPassword.IsPassword = true;
        BtnPasswordHash.ImageSource = "eye.png";
    }

    private async void AuthorityControl()
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            bool giris = await context.TBLPERSONAUTHORITY.Where(a => a.PersonIND == SqlServices.LoginUserGuid).AnyAsync(a => a.PersonAuthorityID == 1009);
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

    private void Isbussy(bool isbusy)
    {
        ActLoad.IsRunning = isbusy;
        BtnImagePicker.IsEnabled = !isbusy;
        TxtUserName.IsEnabled = !isbusy;
        TxtFirstName.IsEnabled = !isbusy;
        TxtLastName.IsEnabled = !isbusy;
        TxtEmail.IsEnabled = !isbusy;
        TxtPhoneNumber.IsEnabled = !isbusy;
        TxtPassword.IsEnabled = !isbusy;
        BtnPasswordHash.IsEnabled = !isbusy;
        BtnClear.IsEnabled = !isbusy;
        BtnAdd.IsEnabled = !isbusy;
    }
    private void ClearAll()
    {
        ImgImage.Source = null;
        TxtUserName.Text = string.Empty;
        TxtFirstName.Text = string.Empty;
        TxtLastName.Text = string.Empty;
        TxtEmail.Text = string.Empty;
        TxtPhoneNumber.Text = string.Empty;
        TxtPassword.Text = string.Empty;
    }
    private async void BtnImagePicker_Clicked(object sender, EventArgs e)
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

                _addpersonImage = memoryStream.ToArray();
            }
        }
        finally
        {
            Isbussy(false);
        }
    }
    private async void BtnPasswordHash_Clicked(object sender, EventArgs e)
    {
		try
        {
            Isbussy(true);
            if (TxtPassword.IsPassword == true)
            {
                TxtPassword.IsPassword = false;
                BtnPasswordHash.ImageSource = "noneye.png";
            }
            else
            {
                TxtPassword.IsPassword = true;
                BtnPasswordHash.ImageSource = "eye.png";
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
            Isbussy(false);
        }
        finally
        {
            Isbussy(false);
        }
    }

    private async void BtnClear_Clicked(object sender, EventArgs e)
    {
        try
        {
            Isbussy(true);
            ClearAll();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
            Isbussy(false);
        }
        finally
        {
            Isbussy(false);
        }
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        try
        {
            Isbussy(true);

            if (string.IsNullOrWhiteSpace(TxtUserName.Text) ||
                string.IsNullOrWhiteSpace(TxtFirstName.Text) ||
                string.IsNullOrWhiteSpace(TxtLastName.Text) ||
                string.IsNullOrWhiteSpace(TxtEmail.Text) ||
                string.IsNullOrWhiteSpace(TxtPhoneNumber.Text) ||
                string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                await Shell.Current.DisplayAlert("Sistem", "Boþ býrakýlan alanlar var", "Tamam");
                return;
            }

            using (var context = new AppDbContext(SqlServices.SqlConnectionString))
            {
                bool userExists = await context.TBLPERSON.AnyAsync(u => u.UserName == TxtUserName.Text);
                bool emailExists = await context.TBLPERSON.AnyAsync(u => u.Email == TxtEmail.Text);

                if (userExists)
                {
                    await Shell.Current.DisplayAlert("Sistem", "Bu Kullanýcý Adý Zaten Kayýtlý", "Tamam");
                }
                else if (emailExists)
                {
                    await Shell.Current.DisplayAlert("Sistem", "Bu Email Zaten Kayýtlý", "Tamam");
                }
                else
                {
                    var newuser = new TblPerson
                    {
                        IND = Guid.NewGuid(),
                        UserImage = _addpersonImage,
                        UserName = TxtUserName.Text,
                        FirstName = TxtFirstName.Text,
                        LastName = TxtLastName.Text,
                        Email = TxtEmail.Text,
                        PhoneNumber = TxtPhoneNumber.Text,
                        Password = TxtPassword.Text,
                        Durum = "Aktif",
                        UserCreateDate = DateTime.Now
                    };

                    await context.TBLPERSON.AddAsync(newuser);
                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                    {
                        ClearAll();
                        SqlServices.CreatUserGuid = newuser.IND;
                        await Shell.Current.GoToAsync($"{nameof(AddUserAuthorityPage)}");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"SQL Hatasý: {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            string errorMessage = ex.InnerException?.Message ?? ex.Message;
            await Shell.Current.DisplayAlert("Sistem", $"SQL Db Hatasý: {errorMessage}", "Tamam");
            await Clipboard.SetTextAsync(errorMessage);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý: {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        finally
        {
            Isbussy(false);
        }
    }
}