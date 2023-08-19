using MicroMotel.Web.Models;

namespace MicroMotel.Web.Services.Interface
{
    public interface ISignUpService
    {
        Task<bool> SignUpAsync(UserSignUpViewModel usuvm);
    }
}
