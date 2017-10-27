namespace Destiny2.Auth
{
    public class Client
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ApiKey { get; set; }
        public string Origin { get; set; }
        public OauthClientType Type { get; set; }
    }
}