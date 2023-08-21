
using MicroMotel.Services.Reservation.DTOs.RoomRDTOs;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Interface
{
    public interface IRoomRService
    {
        public Task<Response<RoomRDTO>> GetRoomRById(int id);
        public Task<Response<List<RoomRDTO>>> GetAllRoomRs();
        public Task<Response<List<RoomRDTO>>> GetAllRoomRsbyPropertyId(int propertyid);
        public Task<Response<int>> CreateReservation(RoomRCreateDTO roomR);
        public Task<Response<NoContent>> DeleteRoomReservation(int id);
        public Task<Response<List<RoomRDTO>>> GetAllRoomRsbyRoomId(int roomid);
    }
}
