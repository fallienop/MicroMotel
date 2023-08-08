using MicroMotel.Web.Services.Interface;
using System.Net.Http.Headers;

namespace MicroMotel.Web.Handler
{
    public class CCTokenHandler:DelegatingHandler
    {
        private readonly ICCService _ccService;

        public CCTokenHandler(ICCService ccService)
        {
            _ccService = ccService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _ccService.GetToken());   
           var resp= await base.SendAsync(request, cancellationToken);
            if(resp.StatusCode==System.Net.HttpStatusCode.Unauthorized) 
            {
                throw new Exception();
            }
            return resp;
        }
    }
}
