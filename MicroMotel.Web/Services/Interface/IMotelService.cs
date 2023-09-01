using MicroMotel.Web.Models.Motel.Meal;
using MicroMotel.Web.Models.Motel.Property;
using MicroMotel.Web.Models.Motel.Room;

namespace MicroMotel.Web.Services.Interface
{
    public interface IMotelService
    {
        #region Property
        Task<List<PropertyViewModel>> GetAllPropertiesAsync();
        Task<PropertyViewModel> GetPropertybyId(int id);
        Task<int> CreateProperty(PropertyCreateInput pci);
        Task<bool> UpdateProperty(PropertyUpdateModel pum);
        Task<bool> DeleteProperty(int id);
        #endregion

        #region Room
        Task<List<PropertyWithRoomsViewModel>> GetPropertyWithRoomsAsync(int propertyid);
        Task<List<RoomViewModel>> GetAllRoomsAsync();
        Task<RoomViewModel> GetRoomById(int id);
        Task<bool> CreateNewRoom(RoomCreateInput rci);
        Task<bool> UpdateRoom(RoomUpdateModel rum);
        Task<bool> DeleteRoom(int id);
        #endregion

        #region Meal
        Task<List<MealViewModel>> GetAllMealsByPropertyId(int propertyId);
        Task<MealViewModel> GetMealById(int id);
        Task<bool> UpdateMeal(MealUpdateModel mum);
        Task<bool> DeleteMeal(int id);
        Task<bool> CreateNewMeal(MealCreateInput mci); 
        #endregion



    }
}
