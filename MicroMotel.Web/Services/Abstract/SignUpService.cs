using MicroMotel.Web.Models;
using MicroMotel.Web.Services.Interface;
using System.Net.Http.Json;

namespace MicroMotel.Web.Services.Abstract
{
    public class SignUpService : ISignUpService
    {
        private readonly HttpClient _httpClient;

        public SignUpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> SignUpAsync(UserSignUpViewModel usuvm)
        {
            var resp = await _httpClient.PostAsJsonAsync<UserSignUpViewModel>("api/user",usuvm);
            return resp.IsSuccessStatusCode;
        }
    }
}
