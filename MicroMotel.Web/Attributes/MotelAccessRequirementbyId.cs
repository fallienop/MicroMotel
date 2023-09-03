using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MicroMotel.Web.Attributes
{
    public class MotelAccessRequirementbyId:IAuthorizationRequirement
    {
    }

    public class MotelAccessHandler : AuthorizationHandler<MotelAccessRequirementbyId>
    {
        private  readonly IHttpContextAccessor _contextAccessor;

        public MotelAccessHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

      
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MotelAccessRequirementbyId requirement)
        {
           if(context.Resource is HttpContext contextt)
            {
                
                var propid = contextt.GetRouteValue("id")?.ToString();
                var rolesss = contextt.User.FindAll("http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
                var claimvalues = new List<string>();
                foreach(var item in rolesss)
                {
                    claimvalues.Add(item?.Value?.ToUpper() ?? "");  
                }
              
                if (claimvalues.Contains(propid))
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
