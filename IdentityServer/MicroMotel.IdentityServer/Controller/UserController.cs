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
using System;
using System.Collections.Generic;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
        [HttpDelete]
        public async Task<IActionResult> removeaccount(string id)
        {
            var useridclaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(useridclaim.Value);

            var res = await _userManager.DeleteAsync(user);
            
            if (res.Succeeded)
            {
                return Ok();
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePassword(string oldpassword,string newpassword)
        {
            var useridclaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(useridclaim.Value);
         var res=   await _userManager.ChangePasswordAsync(user, oldpassword, newpassword);
            if (res.Succeeded)
            {
                return Ok();
            }

            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateDTO userupdate)
        {
            var useridclaim = User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(useridclaim.Value);
            user.UserName = userupdate.Username;
            user.City = userupdate.City;
            user.Email = userupdate.Email;

           var res= await _userManager.UpdateAsync(user);
            if (res.Succeeded)
            {
                return Ok();
            }

            else
            {
                return BadRequest();
            }

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

        [HttpGet("users")]
        public async Task<IActionResult> getallusers()
        {
            var users = await _userManager.Users.ToListAsync(); // Tüm kullanıcıları çek
            var userdtos=new List<object>();
            foreach (var user in users)
            {
                var userdto = new
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    City = user.City,
                    Email = user.Email,
                    Roles=await _userManager.GetRolesAsync(user),
                };

                userdtos.Add(userdto);
            }
           
          
            return Ok(userdtos);

        }

        
        [HttpPut("changerole/{id}")]
       public async Task<IActionResult> ChangeRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            
            var r=IdentityResult.Success;
            if (user == null)
            {
                return BadRequest();
            }
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var removeResult = await _userManager.RemoveFromRoleAsync(user, "Admin");
                
                if (removeResult.Succeeded)
                {
                    await _userManager.UpdateAsync(user); // Veritabanında değişikliği kaydet
                 
                }
                else
                {
                    return BadRequest();
                }
            }
           else if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
              
                try
                {
                    var addResult = await _userManager.AddToRoleAsync(user, "Admin".Normalize());
                    if (addResult.Succeeded)
                    {
                        await _userManager.UpdateAsync(user); // Veritabanında değişikliği kaydet
                        return Ok();
                    }
                    else
                    {
                        // Rol ekleme işlemi başarısız oldu, hatayı loglama
                        foreach (var error in addResult.Errors)
                        {
                            // Hata mesajını loglama kodu buraya ekleyebilirsiniz.
                            Console.WriteLine(error.Description);
                        }
                        return BadRequest("Failed to add Admin role.");
                    }
                }
                catch (Exception ex)
                {
                    // Rol ekleme işlemi sırasında bir hata oluştu, hatayı loglama
                    Console.WriteLine(ex.Message);
                    return BadRequest("An error occurred while adding Admin role.");
                }
            }

            return Ok();

        }
    }
}
