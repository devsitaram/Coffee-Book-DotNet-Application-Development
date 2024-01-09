using BisleriumCafe.Data.Models;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using QuestPDF.Infrastructure;

namespace BisleriumCafe
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });
            builder.Services.AddMauiBlazorWebView();
            QuestPDF.Settings.License = LicenseType.Community;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
