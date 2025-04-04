﻿using UraniumUI;

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
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFontAwesomeIconFonts();
                    fonts.AddMaterialIconFonts();
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
            builder.Services.AddTransient<MyProfilePage>();
            builder.Services.AddSingleton(new AppDbContext(SqlServices.SqlConnectionString));
            builder.Services.AddTransient<WaiterListViewModel>();
            builder.Services.AddTransient<BekleyenKayitlarPage>();

            return builder.Build();
        }
    }
}
