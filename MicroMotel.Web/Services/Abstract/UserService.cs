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
           
            return await _httpClient.GetFromJsonAsync<UserViewModel>($"api/user/getuser");
        }

        public async Task<string> getusername(string id)
        {
            var response = await _httpClient.GetAsync($"api/User/GetUserName/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();


        }

        public async Task<string> GetUserRole()
        {
            var response = await _httpClient.GetAsync($"api/User/getuserrole/role");
            return await response.Content.ReadAsStringAsync();
        }
    }
}   
