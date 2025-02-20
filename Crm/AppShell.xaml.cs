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

        private void PublicServices_LoginChanged()
        {
            LblUserName.Text = $"{PublicServices.UserFullName}";
            ImgImage.Source = PublicServices.profileImage;
        }
    }
}
