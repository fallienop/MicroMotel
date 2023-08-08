using Humanizer;
using IdentityModel.Client;
using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using System.Globalization;
using System.Security.Claims;
using System.Text.Json;

namespace MicroMotel.Web.Services.Abstract
{
    public class ROPService : IROPService
    {
        #region private readonly
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ServiceURLs _serviceURLs;
        private readonly ClientSettings _clientSettings;
        #endregion

    
        public ROPService(HttpClient httpClient, IHttpContextAccessor contextAccessor, ServiceURLs serviceURLs, ClientSettings clientSettings)
        {
            _httpClient = httpClient;
            _contextAccessor = contextAccessor;
            _serviceURLs = serviceURLs;
            _clientSettings = clientSettings;
        } 
    

        public async Task<TokenResponse> GetAccessTokenByRefreshToken()
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceURLs.IdentityServerURL,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            }) ;
            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var refreshtoken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            RefreshTokenRequest refreshtokenrequest = new()
            {
                ClientId = _clientSettings.Registered.ClientId,
                ClientSecret = _clientSettings.Registered.ClientSecret,
                RefreshToken = refreshtoken,
                Address = discovery.TokenEndpoint
            };

            var token= await _httpClient.RequestRefreshTokenAsync(refreshtokenrequest);
            if (token.IsError)
            {
                throw token.Exception;
            }

            var authenticationtokens = new List<AuthenticationToken>() 
            {
                new AuthenticationToken{Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken{Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}

            };
            var authenticationresult = await _contextAccessor.HttpContext.AuthenticateAsync();
           var props = authenticationresult.Properties;
            props.StoreTokens(authenticationtokens);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, authenticationresult.Principal, props);
            return token;

        }

        public async Task RevokeRefreshToken()
        {
            var discovery =await  _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceURLs.IdentityServerURL,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if(discovery.IsError) 
            { 
                throw discovery.Exception; 
            }

            var refreshtoken =await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            TokenRevocationRequest tokenRevocationRequest = new()
            {
                ClientId = _clientSettings.Registered.ClientId,
                ClientSecret=_clientSettings.Registered.ClientSecret,
                TokenTypeHint="refresh_token",
                Address=discovery.TokenEndpoint,
                Token=refreshtoken
            };
            await _httpClient.RevokeTokenAsync(tokenRevocationRequest);
        }

        public async Task<Response<bool>> SignIn(SigninInput sii)
        {
            var discovery = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = _serviceURLs.IdentityServerURL,
                Policy = new DiscoveryPolicy { RequireHttps = false }
            });

            if (discovery.IsError)
            {
                throw discovery.Exception;
            }

            var roptokenrequest = new PasswordTokenRequest
            {
                ClientId = _clientSettings.Registered.ClientId,
                ClientSecret = _clientSettings.Registered.ClientSecret,
                UserName = sii.UserName,
                Password = sii.Password,
                Address = discovery.TokenEndpoint
            };

            var token=await _httpClient.RequestPasswordTokenAsync(roptokenrequest);
            if (token.IsError)
            {
                var responseContent= await token.HttpResponse.Content.ReadAsStringAsync();
                var errordto = JsonSerializer.Deserialize<ErrorDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Response<bool>.Fail(errordto.errors, 400);
            }

            var userinforequest = new UserInfoRequest
            {
                Token=token.AccessToken,
                Address=discovery.UserInfoEndpoint
            };

            var userinfo = await _httpClient.GetUserInfoAsync(userinforequest);
            if (userinfo.IsError)
            {
                throw userinfo.Exception;
            }
                
            ClaimsIdentity claimsIdentity=new ClaimsIdentity(userinfo.Claims,CookieAuthenticationDefaults.AuthenticationScheme,"name", "Roles");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authenticationprops = new AuthenticationProperties();

            authenticationprops.StoreTokens(new List<AuthenticationToken>() 
            {
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.AccessToken,Value=token.AccessToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.RefreshToken,Value=token.RefreshToken},
                new AuthenticationToken(){Name=OpenIdConnectParameterNames.ExpiresIn,Value=DateTime.Now.AddSeconds(token.ExpiresIn).ToString("o",CultureInfo.InvariantCulture)}
            });

            authenticationprops.IsPersistent = sii.IsRemember;

            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal, authenticationprops);

            return Response<bool>.Success(200);
        }
    }
}
