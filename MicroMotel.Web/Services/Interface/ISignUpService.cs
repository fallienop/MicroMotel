using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models;

namespace MicroMotel.Web.Services.Interface
{
    public interface ISignUpService
    {
        Task<Response<List<string>>> SignUpAsync(UserSignUpViewModel usuvm);
    }
}
