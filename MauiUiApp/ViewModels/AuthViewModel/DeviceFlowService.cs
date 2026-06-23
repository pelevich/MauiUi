using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

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

        public async Task<string> RequestDeviceCodeAsync()
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
            return content;
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
                    using JsonDocument document = JsonDocument.Parse(content);
                    JsonElement result = document.RootElement;
                    return result.GetProperty("access_token").GetString();
                }
                await Task.Delay(5000);
            }

            return "Error";
        }
    }
}
