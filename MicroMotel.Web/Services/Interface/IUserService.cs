using MicroMotel.Web.Models.BaseModels;

namespace MicroMotel.Web.Services.Interface
{
    public interface IUserService
    {
        Task<UserViewModel> GetUser();
    }
}
