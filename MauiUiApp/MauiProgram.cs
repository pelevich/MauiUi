using ApplicationService.Domain;
using ApplicationService.Service.serviceFactory;
using CommunityToolkit.Maui;
using MauiUiApp.ViewModels.AuthViewModel;
using MauiUiApp.ViewModels.MainViewModel;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MauiUiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();


            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("MauiUiApp.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();

            builder.Configuration.AddConfiguration(config);

            //Зарегистрировал доп сервисы для DI 
            //Singleton так нужные одни общие данные
            builder.Services.AddSingleton<IServiceFactory, serviceFactoryForPDF>();
            builder.Services.AddSingleton<IBrowseButton, BrowseButtonFile>();
            builder.Services.AddSingleton<IBrowseButton, BrowseButtonFolder>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<AuthViewModel>();
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<DeviceFlowService>();

            builder.Services.AddTransient<PipeFileRepository>();

            builder.Services.AddSingleton<ActivitySource>(sp => null);
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
