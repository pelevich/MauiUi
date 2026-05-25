using Microsoft.Extensions.DependencyInjection;
using MauiUiApp.Application;

namespace MauiUiApp
{
    public partial class App : Microsoft.Maui.Controls.Application
    {
        private readonly StartWebApi _startWebApi;

        //Реализовал DI
        public App(StartWebApi startWebApi)
        {
            InitializeComponent();
            _startWebApi = startWebApi;
            Task.Run(async () => await _startWebApi.StartAsync());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        ~App()
        {
            _startWebApi?.Dispose();
        }
    }
}