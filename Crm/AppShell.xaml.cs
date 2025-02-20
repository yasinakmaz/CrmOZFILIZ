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
            LblUserName.Text = PublicServices.UserFullName;
        }
    }
}
