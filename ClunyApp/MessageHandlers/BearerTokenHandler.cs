using ClunyApp.Authorization;
using Newtonsoft.Json;
using Shared.Models;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ClunyApp.MessageHandlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IHttpContextAccessor contextAccessor;

        public BearerTokenHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor contextAccessor)
        {
            this.httpClientFactory = httpClientFactory;
            this.contextAccessor = contextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Authorization == null &&
                contextAccessor != null &&
                contextAccessor.HttpContext != null)
            {
                JwtToken token = null;

                string? strTokenObj = contextAccessor.HttpContext.Session.GetString("access_token");

                if (string.IsNullOrEmpty(strTokenObj))
                {
                    token = await Authenticate();
                } else
                {
                    token = JsonConvert.DeserializeObject<JwtToken>(strTokenObj) ?? new JwtToken();
                }

                if (token == null ||
                    string.IsNullOrWhiteSpace(token.AccessToken) ||
                    token.ExpiresAt <= DateTime.UtcNow) 
                {
                    token = await Authenticate();
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
            }

            return await base.SendAsync(request, cancellationToken);
        }
        private async Task<JwtToken> Authenticate()
        {
            var user = contextAccessor?.HttpContext?.User;
            string? userEmail = user?.FindFirst(ClaimTypes.Email)?.Value
                               ?? user?.FindFirst("email")?.Value
                               ?? user?.Identity?.Name;

            if (userEmail != null)
            {
                var httpClient = httpClientFactory.CreateClient("auth");
                var res = await httpClient.PostAsJsonAsync(string.Empty,
                    new Credential { 
                        EmailAddress = userEmail
                    });
                res.EnsureSuccessStatusCode();
                string strJwt = await res.Content.ReadAsStringAsync();
                contextAccessor?.HttpContext?.Session.SetString("access_token", strJwt);

                return JsonConvert.DeserializeObject<JwtToken>(strJwt) ?? new JwtToken();
            }

            return new JwtToken();
        }
    }
}
