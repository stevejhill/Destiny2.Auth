using RestSharp;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Destiny2.Auth
{
    public class Oauth
    {
        private readonly ILogger _logger;

        public Oauth(ILogger<Oauth> logger)
        {
            _logger = logger;
        }

        public Task<UserToken> GetToken(string code, Client client)
        {
            var request = GetRequest(client);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", code);
            return MakeRequest(request);
        }

        public Task<UserToken> RefreshToken(Client client, UserToken userToken)
        {
            var request = GetRequest(client);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", userToken.RefreshToken);
            return MakeRequest(request);
        }

        private Error GetResponseError(IRestResponse response)
        {
            return JsonConvert.DeserializeObject<Error>(response.Content);
        }

        private RestRequest GetRequest(Client client)
        {
            var request = new RestRequest("app/oauth/token/")
            {
                Method = Method.POST
            };
            request.AddParameter("client_id", client.ClientId);
            if (client.Type == OauthClientType.Confidential)
                request.AddParameter("client_secret", client.ClientSecret);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Origin", client.Origin);
            return request;
        }

        private Task<UserToken> MakeRequest(RestRequest request)
        {
            var taskRestResponse = new TaskCompletionSource<UserToken>();

            var baseUrl = "https://www.bungie.net/platform/";
            var restClient = new RestClient(baseUrl);

            restClient.ExecuteAsync(request, response =>
            {
                if (response.ResponseStatus == ResponseStatus.Error)
                {
                    _logger.LogError(response.ErrorException, $"Error with request to {request.Resource}");
                    taskRestResponse.SetException(response.ErrorException);
                    return;
                }

                _logger.LogInformation($"Request to {request.Resource} - http response status code was - {response.StatusCode}");

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var error = GetResponseError(response);
                    var bungieError = new System.Exception($"{error.Code} - {error.Description}");
                    _logger.LogError(bungieError, $"Bungie response error");
                    taskRestResponse.SetException(bungieError);
                }
                else
                {
                    _logger.LogInformation($"Response body - {response.Content}");
                    taskRestResponse.SetResult(JsonConvert.DeserializeObject<UserToken>(response.Content));
                }
            });

            return taskRestResponse.Task;
        }
     }
}
