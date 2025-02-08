namespace Crm
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit(options => options.SetShouldEnableSnackbarOnWindows(true))
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<KayitEklePage>();
            builder.Services.AddTransient<BekleyenKayitlarPage>();
            builder.Services.AddTransient<AddAgreementPage>();
            builder.Services.AddTransient<AddPersonPage>();
            builder.Services.AddTransient<AddUserAuthorityPage>();
            builder.Services.AddTransient<AddRecordProgramPage>();

            return builder.Build();
        }
    }
}
