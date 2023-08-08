using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Interface;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Options;

namespace MicroMotel.Web.Services.Abstract
{
    public class CCService : ICCService
    {
        private readonly ServiceURLs _serviceurls;
        private readonly ClientSettings _clientsettings;
        private readonly IClientAccessTokenCache _cache;
        private readonly HttpClient _httpClient;

        public CCService(IOptions<ServiceURLs> serviceurls, IOptions<ClientSettings> clientsettings, IClientAccessTokenCache cache, HttpClient httpClient)
        {
            _serviceurls = serviceurls.Value;
            _clientsettings = clientsettings.Value;
            _cache = cache;
            _httpClient = httpClient;
        }

        public async Task<string> GetToken()
        {
            var currenttoken = await _cache.GetAsync("nonregisteredtoken");
            if(currenttoken is not null)
            {
                return currenttoken.AccessToken;
            }

            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceurls.IdentityServerURL,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var ClientCredentialRequest = new ClientCredentialsTokenRequest
            {
                ClientId = _clientsettings.UnRegistered.ClientId,
                ClientSecret= _clientsettings.UnRegistered.ClientSecret,
                Address=discovery.TokenEndpoint
            };

            var newtoken=await _httpClient.RequestClientCredentialsTokenAsync(ClientCredentialRequest);
            if (newtoken.IsError)
            {
                throw newtoken.Exception;
            }
            await _cache.SetAsync("nonregisteredtoken", newtoken.AccessToken, newtoken.ExpiresIn);

            return newtoken.AccessToken;
        }
    }
}
