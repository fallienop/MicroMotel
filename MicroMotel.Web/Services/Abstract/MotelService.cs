using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Helpers;
using MicroMotel.Web.Models.Motel.Meal;
using MicroMotel.Web.Models.Motel.Property;
using MicroMotel.Web.Models.Motel.Room;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Services.Abstract
{
    public class MotelService : IMotelService
    {
        private readonly HttpClient _httpClient;
        private readonly IPhotoStockService _photoStockService;
        private readonly PhotoHelper _photohelper;

        public MotelService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photohelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photohelper = photohelper;
        }






            #region Property
        public async Task<List<PropertyViewModel>> GetAllPropertiesAsync()
        {
            var response = await _httpClient.GetAsync($"property");
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var r = await   response.Content.ReadAsStringAsync();
            var resp=await response.Content.ReadFromJsonAsync<Response<List<PropertyViewModel>>>();
            resp.Data.ForEach(x => x.Picture = _photohelper.GetPhotoStockURL(x.Picture));
            return resp.Data;
        }

        public async Task<PropertyViewModel> GetPropertybyId(int id)
        {
            var response = await _httpClient.GetAsync($"property/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var resp = await response.Content.ReadFromJsonAsync<Response<PropertyViewModel>>();
            resp.Data.Picture=_photohelper.GetPhotoStockURL(resp.Data.Picture);
            return resp.Data;
        }

        public async Task<int> CreateProperty(PropertyCreateInput pci)
        {
            var resultphoto = await _photoStockService.UploadPhoto(pci.PhotoFormFile);
            if(resultphoto != null)
            {
                pci.Picture = resultphoto.URL;
            }

            var resp = await _httpClient.PostAsJsonAsync("property", pci);
            var response=await resp.Content.ReadFromJsonAsync<Response<int>>();
            var propid = response.Data;
            return propid;
        }
        public async Task<bool> UpdateProperty(PropertyUpdateModel pum)
        {
            var resultphoto = await _photoStockService.UploadPhoto(pum.PhotoFormFile);
            if (resultphoto != null)
            {
                await _photoStockService.DeletePhoto(pum.Picture);
                pum.Picture = resultphoto.URL;
            }
            var resp = await _httpClient.PutAsJsonAsync("property", pum);
            return resp.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteProperty(int id)
        {
            var response = await _httpClient.DeleteAsync($"property/delete/{id}");
            return response.IsSuccessStatusCode;

        }

        
        #endregion


        #region Room

        public async Task<List<PropertyWithRoomsViewModel>> GetPropertyWithRoomsAsync(int propertyid)
        {

            var response = await _httpClient.GetAsync($"room/combined/{propertyid}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var resp = await response.Content.ReadFromJsonAsync<Response<List<PropertyWithRoomsViewModel>>>();
            return resp.Data;
        }
        public async Task<List<RoomViewModel>> GetAllRoomsAsync()
        {
            var response = await _httpClient.GetAsync("room");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var resp=await response.Content.ReadFromJsonAsync<Response<List<RoomViewModel>>>();
            return resp.Data;
            
        }
        public async Task<RoomViewModel> GetRoomById(int id)
        {
            var response = await _httpClient.GetAsync($"room/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null; 
            }

            var resp=await response.Content.ReadFromJsonAsync<Response<RoomViewModel>>();
            return resp.Data;
        }

        public async Task<bool> CreateNewRoom(RoomCreateInput rci)
        {
            var resp = await _httpClient.PostAsJsonAsync("room", rci);
            return resp.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateRoom(RoomUpdateModel rum)
        {
            var resp = await _httpClient.PutAsJsonAsync("room", rum);
            return resp.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteRoom(int id)
        {
            var resp = await _httpClient.DeleteAsync($"room/delete/{id}");
            return resp.IsSuccessStatusCode;
        }
        #endregion


        #region Meal 
        public async Task<List<MealViewModel>> GetAllMealsByPropertyId(int propertyId)
        {
            var response = await _httpClient.GetAsync($"meal/property/{propertyId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var meals=await response.Content.ReadFromJsonAsync<Response<List<MealViewModel>>>();
            return meals.Data;
        }
        public async Task<MealViewModel> GetMealById(int id)
        {
            var response = await _httpClient.GetAsync($"meal/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;

            }
            var meal = await response.Content.ReadFromJsonAsync<Response<MealViewModel>>();
            return meal.Data;
        }
        public async Task<bool> CreateNewMeal(MealCreateInput mci)
        {
            var response = await _httpClient.PostAsJsonAsync("meal", mci);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> UpdateMeal(MealUpdateModel mum)
        {
            var response = await _httpClient.PutAsJsonAsync("meal", mum);
            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteMeal(int id)
        {
            var response = await _httpClient.DeleteAsync($"meal/deletebyid/{id}");
            return response.IsSuccessStatusCode;    
        } 
        #endregion


    }
}
