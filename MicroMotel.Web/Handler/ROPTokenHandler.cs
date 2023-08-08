using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Net.Http.Headers;

namespace MicroMotel.Web.Handler
{
    public class ROPTokenHandler:DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<ROPTokenHandler> _logger;
        private readonly IROPService _ropservice;

        public ROPTokenHandler(IHttpContextAccessor contextAccessor, ILogger<ROPTokenHandler> logger, IROPService ropservice)
        {
            _contextAccessor = contextAccessor;
            _logger = logger;
            _ropservice = ropservice;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accesstoken = await _contextAccessor.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accesstoken);
          var response=await base.SendAsync(request, cancellationToken);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var tokenresponse = await _ropservice.GetAccessTokenByRefreshToken();
                if(tokenresponse != null)
                { 
                   request.Headers.Authorization=new AuthenticationHeaderValue("Bearer",tokenresponse.AccessToken);
                    response = await base.SendAsync(request, cancellationToken);
                }

            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception();
            }
            return response;
        }
    }
}
