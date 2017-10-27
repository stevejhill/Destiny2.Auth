using Newtonsoft.Json;

namespace Destiny2.Auth
{
    public class UserToken
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }
        [JsonProperty("token_type")]
        public string Type { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("refresh_expires_in")]
        public int? RefreshExpiresIn { get; set; }
        [JsonProperty("membership_id")]
        public string MembershipId { get; set; }
    }
}
