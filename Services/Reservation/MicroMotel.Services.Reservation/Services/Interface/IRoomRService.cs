
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Interface
{
    public interface IRoomRService
    {
        public Task<RoomR> GetRoomRById(int id);
        public Task<List<RoomR>> GetAllRoomRs();
        public Task<NoContent> CreateReservation(RoomR roomR);
        public Task<NoContent> UpdateReservation(RoomR roomR);
        public Task<NoContent> DeleteRoomReservation(int id); 
    }
}
