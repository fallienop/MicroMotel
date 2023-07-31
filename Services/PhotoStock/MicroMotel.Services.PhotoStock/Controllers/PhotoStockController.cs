using MicroMotel.Services.PhotoStock.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MicroMotel.Shared.ControllerBases;
using MicroMotel.Shared.DTOs;
namespace MicroMotel.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoStockController : CustomControllerr
    {
        [HttpPost]
        public async Task<IActionResult> Save(IFormFile photo, CancellationToken token)
        {
            if (photo != null && photo.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photo.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken: token);
                return NoContent();
            }
            return CustomActionResult(Response<PhotosDTO>.Fail("empty",400));
        }
       
        public async Task<IActionResult> Delete(string url)
        {
            var dpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", url);
            if(!System.IO.File.Exists(dpath))
            {
                return CustomActionResult(Response<PhotosDTO>.Fail("not found", 404));
            }
            System.IO.File.Delete(dpath);
            return CustomActionResult(Response<PhotosDTO>.Success(200)) ;
        }
    }
}
