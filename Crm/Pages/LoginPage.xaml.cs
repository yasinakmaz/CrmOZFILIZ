namespace Crm.Pages;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
    }

    private void IsBusy(bool isBusy)
    {
        ActLoad.IsRunning = isBusy;
        TxtEmail.IsEnabled = !isBusy;
        TxtPassword.IsEnabled = !isBusy;
        BtnLogin.IsEnabled = !isBusy;
    }

    private async void BtnLogin_Clicked(object sender, EventArgs e)
    {
        try
        {
            IsBusy(true);
            if (!string.IsNullOrWhiteSpace(TxtEmail.Text) || !string.IsNullOrWhiteSpace(TxtPassword.Text))
            {
                using (var context = new AppDbContext(SqlServices.SqlConnectionString))
                {
                    string girilenemail = TxtEmail.Text;

                    bool isEmail = girilenemail.Contains("@");

                    TblPerson user = null;

                    if (isEmail)
                    {
                        user = await context.TBLPERSON.FirstOrDefaultAsync(u => u.Email == girilenemail);

                        if (user == null)
                        {
                            await Shell.Current.DisplayAlert("Sistem", "Email Doðru Deðil", "Tamam");
                            return;
                        }

                        if (user.Password != TxtPassword.Text)
                        {
                            await Shell.Current.DisplayAlert("Sistem", "Þifre Doðru Deðil", "Tamam");
                            return;
                        }
                    }
                    else
                    {
                        user = await context.TBLPERSON.FirstOrDefaultAsync(u => u.UserName == girilenemail);

                        if (user == null)
                        {
                            await Shell.Current.DisplayAlert("Sistem", "Kullanýcý Adý Doðru Deðil", "Tamam");
                            return;
                        }

                        if (user.Password != TxtPassword.Text)
                        {
                            await Shell.Current.DisplayAlert("Sistem", "Parola Doðru Deðil", "Tamam");
                            return;
                        }
                    }

                    SqlServices.LoginUserGuid = user.IND;
                    PublicServices.UserFirstName = user.FirstName;
                    PublicServices.UserLastName = user.LastName;
                    PublicServices.UserFullName = $"{PublicServices.UserFirstName} {PublicServices.UserLastName}";
                    if (user.UserImage != null && user.UserImage.Length > 0)
                    {
                        PublicServices.profileImage = ImageSource.FromStream(() => new MemoryStream(user.UserImage));
                    }
                    await Shell.Current.GoToAsync($"{nameof(KayitEklePage)}");
                    PublicServices.PushLoginChanged(true);
                }
            }
            else
            {
                await Shell.Current.DisplayAlert("Sistem", "Gerekli Alanlar Doldurulmalýdýr", "Tamam");
                IsBusy(false);
            }
        }
        catch (SqlException ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        finally
        {
            IsBusy(false);
        }
    }
}