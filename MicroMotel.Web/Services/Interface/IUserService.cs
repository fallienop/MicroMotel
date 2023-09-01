using MicroMotel.Web.Models;
using MicroMotel.Web.Models.BaseModels;

namespace MicroMotel.Web.Services.Interface
{
    public interface IUserService
    {
        Task<string> getusername(string id);
        Task<UserViewModel> GetUser();
        Task<string> GetUserRole();
        Task<List<UserViewModel>> GetAllUsers();
        Task<bool> ChangeRole(UserRoleChangerViewModel rc);
        Task<bool> AddBalance(UserUpdateModel uum);
        Task<bool> DeleteUser();
        Task<bool> UpdateUser(UserUpdateModel user);
        Task<bool> ChangePassword(string oldPassword, string newPassword);
        Task<bool> AddMotelRole(int id);
        Task<bool> DeleteMotelRole(int id);
    }
}
