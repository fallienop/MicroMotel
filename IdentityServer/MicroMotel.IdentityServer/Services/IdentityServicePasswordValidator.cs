using IdentityModel;
using IdentityServer4.Validation;
using MicroMotel.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace MicroMotel.IdentityServer.Services
{
    public class IdentityServicePasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<ApplicationUser> _usermanager;

        public IdentityServicePasswordValidator(UserManager<ApplicationUser> usermanager)
        {
            _usermanager = usermanager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var existuser=await _usermanager.FindByNameAsync(context.UserName);
            if (existuser == null)
            {
                var errors =new  Dictionary<string, object>();
                errors.Add("errors",new List<string>() { "email is incorrect"});
                context.Result.CustomResponse=errors;
                return;
            }
            var passwordcheck=await _usermanager.CheckPasswordAsync(existuser, context.Password);
            if (passwordcheck == false)
            {
                var errors = new Dictionary<string, object>();
                errors.Add("errors", new List<string>() { "password is incorrect" });
                context.Result.CustomResponse = errors;
                return;
            }
            context.Result = new GrantValidationResult(existuser.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
        }
    }
}
