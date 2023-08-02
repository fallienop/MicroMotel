
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Interface
{
    public interface IRoomRService
    {
        public Task<Response<RoomR>> GetRoomRById(int id);
        public Task<Response<List<RoomR>>> GetAllRoomRs();
        public Task<Response<NoContent>> CreateReservation(RoomR roomR);
        public Task<Response<NoContent>>UpdateReservation(RoomR roomR);
        public Task<Response<NoContent>> DeleteRoomReservation(int id); 
    }
}
