    namespace Crm.Pages;

public partial class AddStatusPage : ContentPage
{
    private byte[] _addpersonImage;
    private bool _addperson;
    public AddStatusPage()
	{
		InitializeComponent();
	}
    private void Isbussy(bool ýsbusy)
    {
        ActLoad.IsRunning = ýsbusy;
        TxtStatusName.IsEnabled = !ýsbusy;
        TxtStatusTitle.IsEnabled = !ýsbusy;
        SldGreen.IsEnabled = !ýsbusy;
        SldBlue.IsEnabled = !ýsbusy;
        SldRed.IsEnabled = !ýsbusy;
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

    private void Color_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Slider target = sender as Slider;
        int v = (int)target.Value;

        if (target.ThumbColor == Colors.Red)
        {
            LblRed.Text = v.ToString();
        }
        else if (target.ThumbColor == Colors.Green)
        {
            LblGreen.Text = v.ToString();
        }
        else if (target.ThumbColor == Colors.Blue)
        {
            LblBlue.Text = v.ToString();
        }

        UpdateSwitch();
    }

    private void UpdateSwitch()
    {
        int r = int.TryParse(LblRed.Text, out int red) ? red : 0;
        int g = int.TryParse(LblGreen.Text, out int green) ? green : 0;
        int b = int.TryParse(LblBlue.Text, out int blue) ? blue : 0;

        BrdShow.BackgroundColor = Color.FromRgb(r, g, b);
        LblHexColor.Text = $"#{r:X2}{g:X2}{b:X2}";
    }
}