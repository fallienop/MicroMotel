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
        Task<List<RoomRViewModel>> GetAllByUserId(string userid);
        Task<RoomRViewModel> GetRoomRById(int id);
        Task<int> NewRoomReservation(RoomRCreateInput input);
        Task<bool> DeleteRoomReservation(int id);
        

        #endregion


        #region Meal reservation

        Task<List<MealRViewModel>> GetAllMealRs();
        Task<List<MealRViewModel>> GetAllMealRsByProperty(int propertyid);
        Task<List<MealRViewModel>> GetAllMealRsByRoom(int roomid);
        Task<MealRViewModel> GetMealRById(int id);
        Task<bool> NewMealReservation(MealRCreateInput input);
        Task<bool> DeleteMealReservation(int id); 

        #endregion




        


    }
}
