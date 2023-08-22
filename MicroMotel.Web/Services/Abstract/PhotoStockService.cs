using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models.PhotoStock;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Services.Abstract
{

    public class PhotoStockService : IPhotoStockService
    {
        private readonly HttpClient _httpClient;

        public PhotoStockService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> DeletePhoto(string photourl)
        {
            var response=await _httpClient.DeleteAsync($"photostock?photourl={photourl}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PhotoViewModel> UploadPhoto(IFormFile photo)
        {
            if (photo == null||photo.Length<=0)
            {
                return null; 
            }
            var randomfilename=$"{Guid.NewGuid().ToString()}{Path.GetExtension(photo.FileName)}";
            using var memorystream=new MemoryStream();
            await photo.CopyToAsync(memorystream);
            var multipart = new MultipartFormDataContent();
            multipart.Add(new ByteArrayContent(memorystream.ToArray()),"photo",randomfilename);
            var response = await _httpClient.PostAsync("photostock", multipart);
            if(!response.IsSuccessStatusCode) 
            {
                return null;
            }
           var responseres= await response.Content.ReadFromJsonAsync<Response<PhotoViewModel>>();
            return responseres.Data;
        }
    }
}
