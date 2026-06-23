using MauiUiApp.ViewModels;

namespace MauiUiApp;

public partial class AuthPage : ContentPage
{
	public AuthPage(AuthViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}