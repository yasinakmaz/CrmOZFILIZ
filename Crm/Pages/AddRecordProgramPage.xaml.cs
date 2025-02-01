namespace Crm.Pages;

public partial class AddRecordProgramPage : ContentPage
{
    public AddRecordProgramPage()
	{
		InitializeComponent();
	}
    private byte[] _programResimByteArray;
    private void ClearAll()
	{
		TxtProgramAdi.Text = string.Empty;
		ImgImage.Source = null;
	}

    private void BtnClear_Clicked(object sender, EventArgs e)
    {
		ClearAll();
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        await SqlServices.InitializeAsync();
        string sqlservices = SqlServices.SqlConnectionString;

        if (string.IsNullOrWhiteSpace(TxtProgramAdi.Text) || _programResimByteArray == null)
        {
            await DisplayAlert("Hata", "L�tfen t�m alanlar� doldurun!", "Tamam");
            return;
        }

        var yeniProgram = new TblProgram
        {
            ID = Guid.NewGuid(),
            ProgramAdi = TxtProgramAdi.Text,
            ProgramResim = _programResimByteArray
        };

        using (var context = new AppDbContext(sqlservices))
        {
            context.TBLPROGRAM.Add(yeniProgram);
            await context.SaveChangesAsync();
        }

        await DisplayAlert("Ba�ar�l�", "Program ba�ar�yla eklendi!", "Tamam");

        ClearAll();
    }

    private async void BtnPrg_Clicked(object sender, EventArgs e)
    {
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
}