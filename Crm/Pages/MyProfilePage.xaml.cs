using System.Threading.Tasks;

namespace Crm.Pages;

public partial class MyProfilePage : ContentPage
{
	public MyProfilePage()
	{
		InitializeComponent();
	}
    private byte[] personýmage;
    protected async override void OnAppearing()
    {
        base.OnAppearing();
		await PushLoad();
    }

    private void Isbussy(bool isbusy)
    {
        ActLoad.IsRunning = isbusy;
        BtnPickImage.IsEnabled = !isbusy;
        TxtUserName.IsEnabled = !isbusy;
        TxtFirstName.IsEnabled = !isbusy;
        TxtLastName.IsEnabled = !isbusy;
        TxtEmail.IsEnabled = !isbusy;
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
				TxtUserName.Text = user.UserName;
				TxtFirstName.Text = user.FirstName;
				TxtLastName.Text = user.LastName;
				TxtEmail.Text = user.Email;
				TxtPhoneNumber.Text = user.PhoneNumber;
				TxtPassword.Text = user.Password;
			}
			else
			{
				await Shell.Current.DisplayAlert("Sistem", "Kullanýcý Bulunamadý", "Tamam");
			}
		}
	}

    private async void BtnPickImage_Clicked(object sender, EventArgs e)
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

                personýmage = memoryStream.ToArray();
            }
        }
        finally
        {
            Isbussy(false);
        }
    }

    private async Task BtnAdd_Clicked(object sender, EventArgs e)
    {
        using (var context = new AppDbContext(SqlServices.SqlConnectionString))
        {
            await context.TBLPERSON.Where(a => a.IND == SqlServices.LoginUserGuid)
        }
    }
}