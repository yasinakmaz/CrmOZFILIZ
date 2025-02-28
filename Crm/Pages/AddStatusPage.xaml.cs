    namespace Crm.Pages;

public partial class AddStatusPage : ContentPage
{
    private byte[] _addpersonImage;
    private bool _addperson;
    private bool addImage;
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
                addImage = true;
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

    private async Task PushData()
    {
        try
        {
            Isbussy(true);

            if(TxtStatusName.Text != string.Empty || TxtStatusTitle.Text != string.Empty || addImage == false)
            {
                var newStatus = new TblStatus
                {
                    IND = Guid.NewGuid(),
                    TitleText = TxtStatusTitle.Text,
                    StatusName = TxtStatusName.Text,
                    StatusImage = _addpersonImage,
                    Order = TxtStatusOrder.Text,
                    ColorCode = LblHexColor.Text,
                };
                using (var context = new AppDbContext(SqlServices.SqlConnectionString))
                {
                    var status = await context.TBLSTATUS.AddAsync(newStatus);
                    int ekle = await context.SaveChangesAsync();
                    if (ekle > 0)
                    {
                        await Shell.Current.DisplayAlert("Sistem", "Kayýt Eklendi", "Tamam");
                    }
                }
            }
        }
        catch (SqlException ex)
        {
            await DisplayAlert("Sistem", $"Sql Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sistem", $"Sistem Hatasý : {ex.Message}", "Tamam");
            await Clipboard.SetTextAsync(ex.Message);
        }
        finally
        {
            Isbussy(false);
        }
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        await PushData();
    }
}