using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroMotel.Web.Attributes
{
    public class MotelAccessRequirementbyId:IAuthorizationRequirement
    {
    }

    public class MotelAccessHandler : AuthorizationHandler<MotelAccessRequirementbyId>
    {
        public MotelAccessHandler()
        {
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MotelAccessRequirementbyId requirement)
        {
           if(context.Resource is AuthorizationFilterContext authcontext)
            {
                var propid = (int)authcontext.RouteData.Values["id"];

                if (context.User.IsInRole(propid.ToString()))
                {
                    context.Succeed(requirement);
                }
                else
                {
                    context.Fail();

                }



            }
           return Task.CompletedTask;
        }
    }
}
