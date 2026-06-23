using MauiUiApp.ViewModels.AuthViewModel;

namespace MauiUiApp;

public partial class AuthPage : ContentPage
{
	public AuthPage(AuthViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}