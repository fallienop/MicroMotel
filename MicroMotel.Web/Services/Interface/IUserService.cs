using MicroMotel.Web.Models.BaseModels;

namespace MicroMotel.Web.Services.Interface
{
    public interface IUserService
    {
        Task<string> getusername(string userid);
        Task<UserViewModel> GetUser();
    }
}
