using ApplicationService.Service.serviceFactory;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using System.Text.Json;

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
            var json_result = await _auth.RequestDeviceCodeAsync();
            using JsonDocument document = JsonDocument.Parse(json_result);
            JsonElement result = document.RootElement;

            UrlAuth = result.GetProperty("verification_uri").GetString();
            UserCode = result.GetProperty("user_code").GetString();

            _acces_token = await _auth.GetTokenAsync(result.GetProperty("device_code").GetString(), result.GetProperty("expires_in").GetInt32(), result.GetProperty("interval").GetInt32());
            await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
        }

        [RelayCommand]
        private async Task OpenUrl(string url)
        {
            url += $"?user_code={userCode}";
            await Launcher.OpenAsync(url);
        }
    }
}
