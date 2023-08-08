using IdentityModel.Client;
using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models.BaseModels;

namespace MicroMotel.Web.Services.Interface
{
    public interface IROPService
    {
        Task<Response<bool>> SignIn(SigninInput sii);
        Task<TokenResponse> GetAccessTokenByRefreshToken();
        Task RevokeRefreshToken();
        

    }
}
