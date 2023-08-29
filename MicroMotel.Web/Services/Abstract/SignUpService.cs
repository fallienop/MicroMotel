using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models;
using MicroMotel.Web.Services.Interface;
using Microsoft.Extensions.Logging.Abstractions;

namespace MicroMotel.Web.Services.Abstract
{
    public class SignUpService : ISignUpService
    {
        private readonly HttpClient _httpClient;

        public SignUpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Response<List<string>>> SignUpAsync(UserSignUpViewModel usuvm)
        {
            var resp = await _httpClient.PostAsJsonAsync<UserSignUpViewModel>("api/user/newuser",usuvm);
          var respp=await resp.Content.ReadAsStringAsync();
            
            var response = await resp.Content.ReadFromJsonAsync<Response<List<string>>>();
            
            return response;
        }
    }
}
