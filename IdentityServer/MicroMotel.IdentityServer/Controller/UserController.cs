using IdentityModel;
using MicroMotel.IdentityServer.DTOs;
using MicroMotel.IdentityServer.Models;
using MicroMotel.Shared.ControllerBases;
using MicroMotel.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace MicroMotel.IdentityServer.Controller
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : CustomControllerr
    {
        private readonly UserManager<ApplicationUser> _userManager;
   

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var useridclaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (useridclaim == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(useridclaim.Value);
            if (user == null)
            {
                return BadRequest();
            }
            var role = await _userManager.GetRolesAsync(user);
            return Ok(new { Id = user.Id, UserName = user.UserName, City = user.City, Email = user.Email });

        }



        [HttpPost]
        public async Task<IActionResult> NewUser(SignUpDTO sud)
        {
            var user = new ApplicationUser()
            {
                UserName = sud.Username,
                City = sud.City,
                Email = sud.Email
            };
            var res = await _userManager.CreateAsync(user, sud.Password);
            if (!res.Succeeded)
            {
                return BadRequest(Response<NoContent>.Fail(res.Errors.Select(x => x.Description).ToList(), 404));
            }
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserName(string id)
        {
            var user=await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return BadRequest();
            }
           
            return Ok(user.UserName);
           
        }

        [HttpGet("role")]
        public async Task<IActionResult> getuserrole()
        {
            var useridclaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            if (useridclaim == null)
            {
                return BadRequest();
            }
            var user = await _userManager.FindByIdAsync(useridclaim.Value);
            if (user == null)
            {
                return BadRequest();
            }
            var role = await _userManager.GetRolesAsync(user);
            return Ok(role);
        }
    }
}
