using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Services.Abstract
{
    public class UserService:IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserViewModel> GetUser()
        {
            return await _httpClient.GetFromJsonAsync<UserViewModel>("api/user/getuser");
        }

        public async Task<string> getusername(string userid)
        {
            //var res=await _httpClient.GetAsync($"api/user/GetUserName/{userid}");

            //return res.StatusCode.ToString();
            return ""; 

        }
    }
}   
