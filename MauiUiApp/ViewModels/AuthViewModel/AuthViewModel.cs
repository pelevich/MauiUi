using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MauiUiApp.ViewModels.AuthViewModel
{
    public partial class AuthViewModel : ObservableObject
    {
        private readonly DeviceFlowService _auth;
        private string _acces_token;
        [ObservableProperty]
        private string urlAuth;
        [ObservableProperty]
        private string userCode;

        public AuthViewModel(DeviceFlowService auth)
        {
            _auth = auth;
        }

        [RelayCommand]
        private async Task Auth()
        {
            var result = await _auth.RequestDeviceCodeAsync();

            UrlAuth = result.VerificationUri;
            UserCode = result.UserCode;

            _acces_token = await _auth.GetTokenAsync(result.DeviceCode, int.Parse(result.ExpiresIn), int.Parse(result.Interval));
            if(_acces_token != "Error") await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        [RelayCommand]
        private async Task OpenUrl(string url)
        {
            url += $"?user_code={userCode}";
            await Launcher.OpenAsync(url);
        }
    }
}
