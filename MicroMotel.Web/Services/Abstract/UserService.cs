﻿using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models;
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

        public async Task<bool> AddBalance(UserUpdateModel uum)
        {
            var response=await _httpClient.PutAsJsonAsync<UserUpdateModel>($"api/User/AddBalance",uum);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetUserRole()
        {
            var response = await _httpClient.GetAsync($"api/User/getuserrole/role");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<List<UserViewModel>> GetAllUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<UserViewModel>>("api/User/getallusers/users");
        }

        public async Task<bool> ChangeRole(UserRoleChangerViewModel rc)
        {
            var response = await _httpClient.PostAsJsonAsync<UserRoleChangerViewModel>($"/api/User/ChangeRole",rc);
          return  response.IsSuccessStatusCode;
          

        }

        public async Task<bool> DeleteUser()
        {
            var r = await _httpClient.DeleteAsync("api/user/removeaccount");
            return r.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateUser(UserUpdateModel user)
        {
            var r = await _httpClient.PutAsJsonAsync<UserUpdateModel>("api/user/UpdateUser", user);
            return r.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePassword(string oldPassword, string newPassword)
        {
            var data = new Dictionary<string, string>
        {
            { "oldpassword", oldPassword },
            { "newpassword", newPassword }
        };

            var content = new FormUrlEncodedContent(data);
            var r = await _httpClient.PutAsync("api/user/UpdatePassword",content);
            return r.IsSuccessStatusCode;

        }

        public async Task<bool> AddMotelRole(int id)
        {
           
            var r = await _httpClient.PostAsync($"/api/user/AddMotelRole/{id}",null);
            return r.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMotelRole(int id)
        {
            var r = await _httpClient.DeleteAsync($"/api/user/DeleteMotelRole/{id}");
            return r.IsSuccessStatusCode;
        }
    }
}   
