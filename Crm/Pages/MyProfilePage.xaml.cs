namespace Crm.Pages;

public partial class MyProfilePage : ContentPage
{
	public MyProfilePage()
	{
		InitializeComponent();
	}
    private byte[]? personýmage;
    private bool? ImageChange,UserNameChange,FirstNameChange,LastNameChange,EmailChange,PhoneNumberChange,PasswordChange;

    private string? UserName = string.Empty,FirstName = string.Empty, LastName = string.Empty, Email = string.Empty, PhoneNumber = string.Empty, Password = string.Empty;
    protected async override void OnAppearing()
    {
        base.OnAppearing();
		await PushLoad();
        await Getir();
    }

    private async void BtnLogOut_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("//LoginPage");
            App.Current.MainPage = new AppShell();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
        }
    }

    private void Isbussy(bool isbusy)
    {
        ActLoad.IsRunning = isbusy;
        BtnPickImage.IsEnabled = !isbusy;
        TxtFirstName.IsEnabled = !isbusy;
        TxtLastName.IsEnabled = !isbusy;
        TxtPhoneNumber.IsEnabled = !isbusy;
        TxtPassword.IsEnabled = !isbusy;
        BtnAdd.IsEnabled = !isbusy;
    }

    private async Task PushLoad()
	{
		using (var context = new AppDbContext(SqlServices.SqlConnectionString))
		{
			var user = await context.TBLPERSON.FindAsync(SqlServices.LoginUserGuid);
			if(user != null)
			{
				ImgImage.Source = PublicServices.profileImage;
				UserName = user.UserName;
				FirstName = user.FirstName;
				LastName = user.LastName;
				Email = user.Email;
				PhoneNumber = user.PhoneNumber;
				Password = user.Password;
			}
			else
			{
				await Shell.Current.DisplayAlert("Sistem", "Kullanýcý Bulunamadý", "Tamam");
			}
		}
	}

    private async Task Getir()
    {
        TxtUserName.Text = UserName;
        TxtFirstName.Text = FirstName;
        TxtLastName.Text = LastName;
        TxtEmail.Text = Email;
        TxtPhoneNumber.Text = PhoneNumber;
        TxtPassword.Text = Password;
    }

    private async void BtnPickImage_Clicked(object sender, EventArgs e)
    {
        try
        {
            ImageChange = true;
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

                personýmage = memoryStream.ToArray();
            }
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
            using (var context = new AppDbContext(SqlServices.SqlConnectionString))
            {
                var user = await context.TBLPERSON.FindAsync(SqlServices.LoginUserGuid);
                if (user == null) return;

                bool isUpdated = false;

                if (ImageChange == true)
                {
                    user.UserImage = personýmage;
                    isUpdated = true;
                }

                if (UserNameChange == true)
                {
                    user.UserName = TxtUserName.Text;
                    isUpdated = true;
                }

                if (FirstNameChange == true)
                {
                    user.FirstName = TxtFirstName.Text;
                    isUpdated = true;
                }

                if (LastNameChange == true)
                {
                    user.LastName = TxtLastName.Text;
                    isUpdated = true;
                }

                if (EmailChange == true)
                {
                    user.Email = TxtEmail.Text;
                    isUpdated = true;
                }

                if (PhoneNumberChange == true)
                {
                    user.PhoneNumber = TxtPhoneNumber.Text;
                    isUpdated = true;
                }

                if (PasswordChange == true)
                {
                    user.Password = TxtPassword.Text;
                    isUpdated = true;
                }

                if (isUpdated)
                {
                    await context.SaveChangesAsync();
                    if (user.UserImage != null && user.UserImage.Length > 0)
                    {
                        PublicServices.profileImage = ImageSource.FromStream(() => new MemoryStream(user.UserImage));
                    }
                    PublicServices.PushLoginChanged(true);
                }
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        catch (DbUpdateException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Db Update Sql Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        finally
        {
            Isbussy(false);
        }
    }
    private void TxtUserName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if(UserName != TxtUserName.Text)
        {
            UserNameChange = true;
        }
        else
        {
            UserNameChange = false;
        }
    }

    private void TxtFirstName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (FirstName != TxtFirstName.Text)
        {
            FirstNameChange = true;
        }
        else
        {
            FirstNameChange = false;
        }
    }

    private void TxtLastName_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (LastName != TxtLastName.Text)
        {
            LastNameChange = true;
        }
        else
        {
            LastNameChange = false;
        }
    }

    private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Email != TxtEmail.Text)
        {
            EmailChange = true;
        }
        else
        {
            EmailChange = false;
        }
    }

    private void TxtPhoneNumber_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (PhoneNumber != TxtPhoneNumber.Text)
        {
            PhoneNumberChange = true;
        }
        else
        {
            PhoneNumberChange = false;
        }
    }

    private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (Password != TxtPassword.Text)
        {
            PasswordChange = true;
        }
        else
        {
            PasswordChange = false;
        }
    }
}