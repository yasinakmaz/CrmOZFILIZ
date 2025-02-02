namespace Crm.Pages;

public partial class AddPersonPage : ContentPage
{
	public AddPersonPage()
	{
		InitializeComponent();
        TxtPassword.IsPassword = true;
        BtnPasswordHash.ImageSource = "eye.png";
    }
    private void BtnPasswordHash_Clicked(object sender, EventArgs e)
    {
		if(TxtPassword.IsPassword == true)
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
}