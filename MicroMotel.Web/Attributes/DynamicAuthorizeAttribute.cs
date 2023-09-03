using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MicroMotel.Shared.Services;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
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
        private readonly IUserService _userService;
        private readonly string _requiredRole;

        public DynamicAuthorizeFilter(IUserService userService, string requiredRole)
        {
            _userService = userService;
            _requiredRole = requiredRole;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool isallowanonymous = false;
            if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any()){
                isallowanonymous= true;
            }

            var propid = context.HttpContext.GetRouteValue("id")?.ToString();
            var role = await _userService.GetUserRole();
            var roles = role.Split(',');
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            for (int i = 0; i < roles.Length; i++)
            {
                roles[i] = rgx.Replace(roles[i], "");
            }


            if (!(roles.Contains(_requiredRole)||roles.Contains(propid)||isallowanonymous))
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }


}
