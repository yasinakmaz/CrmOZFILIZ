namespace Crm
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(KayitEklePage), typeof(KayitEklePage));
            Routing.RegisterRoute(nameof(BekleyenKayitlarPage), typeof(BekleyenKayitlarPage));
            Routing.RegisterRoute(nameof(AddAgreementPage), typeof(AddAgreementPage));
            Routing.RegisterRoute(nameof(AddPersonPage), typeof(AddPersonPage));
            Routing.RegisterRoute(nameof(AddUserAuthorityPage), typeof(AddUserAuthorityPage));
            Routing.RegisterRoute(nameof(AddRecordProgramPage), typeof(AddRecordProgramPage));
            Routing.RegisterRoute(nameof(MyProfilePage), typeof(MyProfilePage));
            PublicServices.LoginChanged += PublicServices_LoginChanged;
        }

        private async void PublicServices_LoginChanged()
        {
            LblUserName.Text = $"{PublicServices.UserFullName}";
            ImgImage.Source = PublicServices.profileImage;

            try
            {
                var authorityPages = new Dictionary<int, object>
                    {
                        { 1001, KayitEklePage },
                        { 1004, BekleyenKayitlarPage },
                        { 1005, OnaylanmisKayitlarPage },
                        { 1006, IptalEdilenKayitlarPage },
                        { 1007, AddRecordProgramPage },
                        { 1008, AddAgreementPage },
                        { 1009, AddPersonPages}
                    };

                var context = new AppDbContext(SqlServices.SqlConnectionString);

                var userAuthorities = await context.TBLPERSONAUTHORITY
                .Where(a => a.PersonIND == SqlServices.LoginUserGuid && authorityPages.Keys.Contains(a.PersonAuthorityID))
                .Select(a => a.PersonAuthorityID)
                .ToListAsync();

                foreach (var id in authorityPages.Keys)
                {
                    if (!userAuthorities.Contains(id))
                    {
                        ((dynamic)authorityPages[id]).FlyoutItemIsVisible = false;
                    }
                }
            }
            catch (SqlException ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Sql Hatası : {ex.Message}", "Tamam");
                await Clipboard.SetTextAsync(ex.Message);
            }
            catch (DbUpdateException ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Db Update Sql Hatası : {ex.Message}", "Tamam");
                await Clipboard.SetTextAsync(ex.Message);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Sistem", $"Sistem Hatası : {ex.Message}", "Tamam");
                await Clipboard.SetTextAsync(ex.Message);
            }
        }
    }
}
