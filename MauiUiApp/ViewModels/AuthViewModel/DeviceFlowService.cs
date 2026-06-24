using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MauiUiApp.ViewModels.AuthViewModel
{
    public class DeviceFlowService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _keycloakUrl;
        private readonly string _realm;
        private readonly string _clientId;

        public DeviceFlowService(IConfiguration configuration)
        {
            _keycloakUrl = configuration["Keycloak:Authority"];
            _realm = configuration["Keycloak:Realm"];
            _clientId = configuration["Keycloak:ClientId"];
        }

        public async Task<DeviceCodeRespones> RequestDeviceCodeAsync()
        {
            var url = $"{_keycloakUrl}/realms/{_realm}/protocol/openid-connect/auth/device";
            var parameters = new Dictionary<string, string>
            {
                ["client_id"] = _clientId,
                ["scope"] = "openid profile"
            };

            var response = await _httpClient.PostAsync(url,
                new FormUrlEncodedContent(parameters));

            var content = await response.Content.ReadAsStringAsync();
            var deviceCodeRespones = JsonConvert.DeserializeObject<DeviceCodeRespones>(content);
            return deviceCodeRespones;
        }

        public async Task<string> GetTokenAsync(string deviceCode, int expires_in, int interval)
        {
            var url = $"{_keycloakUrl}/realms/{_realm}/protocol/openid-connect/token";
            string content;
            var parameters = new Dictionary<string, string>
            {
                ["client_id"] = _clientId,
                ["device_code"] = deviceCode,
                ["grant_type"] = "urn:ietf:params:oauth:grant-type:device_code"
            };

            for (int attempt = 0; attempt < expires_in/ interval; attempt++)
            {
                var response = await _httpClient.PostAsync(url,
                    new FormUrlEncodedContent(parameters));

                content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    string token = JObject.Parse(content)["access_token"]?.ToString();
                    return token;
                }
                await Task.Delay(interval*1000);
            }

            return "Error";
        }
    }
}
