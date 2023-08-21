using MicroMotel.Web.Models.Motel.Meal;
using MicroMotel.Web.Models.Reservation.MealR;
using MicroMotel.Web.Models.Reservation.RoomR;

namespace MicroMotel.Web.Services.Interface
{
    public interface IReservationService
    {
     
        #region Room reservation
        Task<List<RoomRViewModel>> GetAll();

        Task<List<RoomRViewModel>> GetAllByPropertyId(int propertyId);
        Task<List<RoomRViewModel>> GetAllByRoomId(int roomId);

        Task<RoomRViewModel> GetRoomById(int id);



        Task<int> NewRoomReservation(RoomRCreateInput input);

        Task<bool> DeleteRoomReservation(int id);
        #endregion

        
        #region Meal reservation
        Task<List<MealViewModel>> GetAllMealRs();
        Task<List<MealViewModel>> GetAllMealsByProperty(int propertyid);
        Task<List<MealViewModel>> GetAllMealsByRoom(int roomid);
        Task<MealViewModel> GetMealById(int id);
        Task<bool> NewMealReservation(MealRCreateInput input);
        Task<bool> DeleteMealReservation(int id); 
        #endregion


    }
}
