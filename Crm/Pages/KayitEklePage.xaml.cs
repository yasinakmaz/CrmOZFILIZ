namespace Crm.Pages;

public partial class KayitEklePage : ContentPage
{
    private byte[] selectedImageBytes;

    public KayitEklePage()
    {
        InitializeComponent();
        Services.SqlServices.InitializeAsync();
        PckUyg.Items.Add("Arctos");
    }

    private protected string query = "INSERT INTO TblPortalBekleyenKayitlar (Secim, AdSoyad, Firma, TelNo, TelNo2, Anydesk, Uyg, Sorun, Kisi, Resim) VALUES (@Secim, @AdSoyad, @Firma, @TelNo, @TelNo2, @Anydesk, @Uyg, @Sorun, @Kisi, @Resim)";

    public class ByteArrayToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ImageSource retSource = null;
            if (value != null)
            {
                byte[] imageAsBytes = (byte[])value;
                var stream = new MemoryStream(imageAsBytes);
                retSource = ImageSource.FromStream(() => stream);
            }
            return retSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    private async void Btnimage_Clicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Bir resim seçin",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                using (var stream = await result.OpenReadAsync())
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    selectedImageBytes = memoryStream.ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sistem", "Resim seçilirken bir hata oluþtu: " + ex.Message, "Tamam");
        }
    }

    private async void BtnClear_Clicked(object sender, EventArgs e)
    {
        Active.IsEnabled = true;
        Active.IsRunning = true;
        try
        {
            TxtAd.Text = "";
            TxtFirma.Text = "";
            TxtTel.Text = "";
            TxtTel2.Text = "";
            TxtAnydesk.Text = "";
            PckUyg.SelectedIndex = 0;
            TxtSorun.Text = "";

            RdbHigh.IsChecked = false;
            RdbMed.IsChecked = false;
            RdbLow.IsChecked = false;

            selectedImageBytes = null;

            Active.IsRunning = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sistem", "Hata :" + ex.Message, "Tamam");
        }
        finally
        {
            Active.IsEnabled = false;
        }
    }

    private async void Clear()
    {
        Active.IsEnabled = true;
        Active.IsRunning = true;
        try
        {
            TxtAd.Text = "";
            TxtFirma.Text = "";
            TxtTel.Text = "";
            TxtTel2.Text = "";
            TxtAnydesk.Text = "";
            PckUyg.SelectedIndex = 0;
            TxtSorun.Text = "";

            RdbHigh.IsChecked = false;
            RdbMed.IsChecked = false;
            RdbLow.IsChecked = false;

            selectedImageBytes = null;

            Active.IsRunning = false;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Sistem", "Hata :" + ex.Message, "Tamam");
        }
        finally
        {
            Active.IsEnabled = false;
        }
    }

    private async void BtnAdd_Clicked(object sender, EventArgs e)
    {
        string selectedPriority = RdbHigh.IsChecked ? "High" : (RdbMed.IsChecked ? "Med" : "Low");
        SqlConnection conn = new SqlConnection(SqlServices.SqlConnectionString);

        try
        {
            await conn.OpenAsync();
            Active.IsRunning = true;
            if (TxtAd.Text == null)
            {
                await DisplayAlert("Sistem", "Ad Ve Soyad Kýsmý Boþ Býrakýlamaz", "Tamam");
            }
            else if (TxtFirma.Text == null)
            {
                await DisplayAlert("Sistem", "Firma Kýsmý Boþ Býrakýlamaz", "Tamam");
            }
            else if (TxtTel.Text == null)
            {
                await DisplayAlert("Sistem", "Telefon Numarasý Boþ Býrakýlamaz", "Tamam");
            }
            else if (TxtSorun.Text == null)
            {
                await DisplayAlert("Sistem", "Müþteri Sorunu Boþ Býrakýlamaz", "Tamam");
            }
            else if (RdbHigh.IsChecked == false && RdbMed.IsChecked == false && RdbLow.IsChecked == false)
            {
                await DisplayAlert("Sistem", "Lütfen Bir Öncelik Seçiniz", "Tamam");
            }
            else
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@Secim", selectedPriority);
                    cmd.Parameters.AddWithValue("@AdSoyad", TxtAd.Text);
                    cmd.Parameters.AddWithValue("@Firma", TxtFirma.Text);
                    cmd.Parameters.AddWithValue("@TelNo", TxtTel.Text);
                    cmd.Parameters.AddWithValue("@TelNo2", TxtTel2.Text);
                    cmd.Parameters.AddWithValue("@Anydesk", TxtAnydesk.Text);
                    cmd.Parameters.AddWithValue("@Uyg", PckUyg.SelectedItem);
                    cmd.Parameters.AddWithValue("@Sorun", TxtSorun.Text);
                    cmd.Parameters.AddWithValue("@Kisi", "");
                    cmd.Parameters.AddWithValue("@Resim", selectedImageBytes);

                    int rowsAffected = await cmd.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {
                        await DisplayAlert("Sistem", "Veri baþarýyla eklendi!", "Tamam");
                        Clear();
                    }
                    else
                    {
                        await DisplayAlert("Sistem", "Veri eklenirken bir hata oluþtu.", "Tamam");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Sistem", "Hata : " + ex.Message, "Tamam");
                }
                finally
                {
                    Active.IsRunning = false;
                }
            }
        }
        catch (SqlException ex)
        {
            await DisplayAlert("Sistem", "Hata :" + ex.Message, "Tamam");
            await Clipboard.Default.SetTextAsync(ex.Message);
        }
        finally
        {
            await conn.CloseAsync();
            Active.IsRunning = false;
        }
    }
}
