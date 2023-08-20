using IdentityModel;
using MicroMotel.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroMotel.IdentityServer.Services
{
    public class ClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ClaimsFactory(
            UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
            _userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var emails = await _userManager.GetEmailAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role));
            var emailclaim = new Claim(ClaimTypes.Email, emails);
            identity.AddClaims(roleClaims);
            identity.AddClaim(emailclaim);
            return identity;
        }
    }
}
