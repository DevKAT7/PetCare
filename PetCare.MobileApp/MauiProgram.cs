using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using PetCare.MobileApp.Auth;
using PetCare.MobileApp.Services;

namespace PetCare.MobileApp
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

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
