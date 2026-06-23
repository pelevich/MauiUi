using MauiUiApp.ViewModels.MainViewModel;

namespace MauiUiApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }
}
