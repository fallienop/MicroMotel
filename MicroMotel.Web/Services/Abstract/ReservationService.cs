using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Models.Motel.Meal;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Services.Abstract
{
    public class ReservationService : IReservationService
    {
        private readonly HttpClient _httpClient;

        public ReservationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        #region RoomR
        public async Task<List<RoomRViewModel>> GetAll()
        {
            var response = await _httpClient.GetAsync($"roomr");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var roomrs = await response.Content.ReadFromJsonAsync<Response<List<RoomRViewModel>>>();
            return roomrs.Data;
        }
        public async Task<RoomRViewModel> GetRoomRById(int id)
        {
            var response = await _httpClient.GetAsync($"roomr/{id}");
            if(!response.IsSuccessStatusCode) 
            {
                return null;
            }

            var roomr=await response.Content.ReadFromJsonAsync<Response<RoomRViewModel>>();
            return roomr.Data;
        }
        public async Task<List<RoomRViewModel>> GetAllByPropertyId(int propertyId)
        {
            var response = await _httpClient.GetAsync($"roomr/prop/{propertyId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var roomrs = await response.Content.ReadFromJsonAsync<Response<List<RoomRViewModel>>>();
            return roomrs.Data;
        }
        public async Task<int> NewRoomReservation(RoomRCreateInput input)
        {
            var response = await _httpClient.PostAsJsonAsync("roomr",input);
            var ints = await response.Content.ReadFromJsonAsync<Response<int>>();
            return ints.Data;
        }
        public async Task<bool> DeleteRoomReservation(int id)
        {
            var response = await _httpClient.DeleteAsync($"roomr/delete/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<RoomRViewModel>> GetAllByRoomId(int roomId)
        {
            var response = await _httpClient.GetAsync($"roomr/room/{roomId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var roomrs = await response.Content.ReadFromJsonAsync<Response<List<RoomRViewModel>>>();
            return roomrs.Data;
        }

        #endregion



        #region MealR

        public async Task<List<MealViewModel>> GetAllMealRs()
        {
            var response = await _httpClient.GetAsync("mealr");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var mealrs=await response.Content.ReadFromJsonAsync<Response<List<MealViewModel>>>();
            return mealrs.Data;
        }
        public async Task<List<MealViewModel>> GetAllMealsByProperty(int propertyid)
        {
            var response = await _httpClient.GetAsync($"mealr/prop/{propertyid}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var mealrs = await response.Content.ReadFromJsonAsync<Response<List<MealViewModel>>>();
            return mealrs.Data;
        }
        public async Task<List<MealViewModel>> GetAllMealsByRoom(int roomid)
        {
            var response = await _httpClient.GetAsync($"mealr/room/{roomid}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var mealrs = await response.Content.ReadFromJsonAsync<Response<List<MealViewModel>>>();
            return mealrs.Data;
        }
        public async Task<MealViewModel> GetMealById(int id)
        {
            var response = await _httpClient.GetAsync($"mealr/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            var mealrs = await response.Content.ReadFromJsonAsync<Response<MealViewModel>>();
            return mealrs.Data;
        }
        public async Task<bool> NewMealReservation(MealRCreateInput input)
        {
            var response = await _httpClient.PostAsJsonAsync("mealr", input);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteMealReservation(int id)
        {
            var response = await _httpClient.DeleteAsync($"mealr/delete/{id}");
            return response.IsSuccessStatusCode;
        }

      
        #endregion









    }
}
