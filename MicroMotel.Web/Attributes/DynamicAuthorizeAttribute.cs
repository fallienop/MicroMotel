using System;
using System.Linq;
using System.Threading.Tasks;
using MicroMotel.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroMotel.Web.Attributes
{
    public class DynamicAuthorizeAttribute : TypeFilterAttribute
    {
        public DynamicAuthorizeAttribute(string requiredRole) : base(typeof(DynamicAuthorizeFilter))
        {
            Arguments = new object[] { requiredRole };
        }
    }

    public class DynamicAuthorizeFilter : IAsyncAuthorizationFilter
    {
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly string _requiredRole;

        public DynamicAuthorizeFilter(ISharedIdentityService sharedIdentityService, string requiredRole )
        {
            _sharedIdentityService = sharedIdentityService;
            _requiredRole = requiredRole;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userrole = _sharedIdentityService.getUserRole;
            if (userrole != _requiredRole)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }


}
