using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Reservation.Services.Abstract
{
    public class RoomRService : IRoomRService
    {
        public Task<NoContent> CreateReservation(RoomR roomR)
        {
            throw new NotImplementedException();
        }

        public Task<NoContent> DeleteRoomReservation(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RoomR>> GetAllRoomRs()
        {
            throw new NotImplementedException();
        }

        public Task<RoomR> GetRoomRById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NoContent> UpdateReservation(RoomR roomR)
        {
            throw new NotImplementedException();
        }
    }
}
