using MicroMotel.Web.Models.PhotoStock;

namespace MicroMotel.Web.Services.Interface
{
    public interface IPhotoStockService
    {
        Task<PhotoViewModel> UploadPhoto(IFormFile photo);
        Task<bool> DeletePhoto(string photourl);
    }
}
