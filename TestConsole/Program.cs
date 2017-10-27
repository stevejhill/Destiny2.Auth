using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(new LoggerFactory().AddConsole());
            serviceCollection.AddLogging();
            serviceCollection.AddTransient<Destiny2.Auth.Oauth>();
            var serviceProvider = serviceCollection.BuildServiceProvider();


            var oauthProvider = serviceProvider.GetService<Destiny2.Auth.Oauth>();


            var client = new Destiny2.Auth.Client()
            {
                ClientId = "YOUR_CLIENT_ID",
                ClientSecret = "YOUR_CLIENT_SECRET",
                ApiKey = "YOUR_API_KEY",
                Origin = "YOUR_ORIGIN",
                Type = Destiny2.Auth.OauthClientType.Confidential
            };

            var userToken = oauthProvider.GetToken("CODE_FROM_POST_BACK_URL", client).Result;
            var newUserToken = oauthProvider.RefreshToken(client, userToken).Result;

            Console.ReadLine();
        }
    }
}
