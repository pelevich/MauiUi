using Newtonsoft.Json;

namespace MauiUiApp.ViewModels.AuthViewModel
{
    public class DeviceCodeRespones
    {
        [JsonProperty("device_code")]
        public string DeviceCode { get; set; }

        [JsonProperty("user_code")]
        public string UserCode { get; set; }

        [JsonProperty("verification_uri")]
        public string VerificationUri { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("interval")]
        public string Interval { get; set; }
    }
}
