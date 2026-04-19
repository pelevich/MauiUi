using MauiUiApp.ViewModels;

namespace MauiUiApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            var vm = new MainViewModel();
            BindingContext = vm;
            InitializeComponent();
        }

    }
}
