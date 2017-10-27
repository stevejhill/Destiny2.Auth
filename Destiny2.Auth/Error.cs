using Newtonsoft.Json;

namespace Destiny2.Auth
{
    public class Error
    {
        [JsonProperty("error")]
        public string Code { get; set; }

        [JsonProperty("error_description")]
        public string Description { get; set; }
    }
}
