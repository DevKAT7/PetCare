using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using PetCare.MobileApp.Auth;
using PetCare.MobileApp.Services;
using Serilog;
using System.Globalization;

namespace PetCare.MobileApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Debug()
            .CreateLogger();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            string baseAddress;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                baseAddress = "http://10.0.2.2:5084/";
            }
            else
            {
                baseAddress = "http://localhost:5084/";
            }

            builder.Services.AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri(baseAddress);
            });

            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
            builder.Services.AddLogging(logging => logging.AddSerilog());

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            var culture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            return builder.Build();
        }
    }
}
