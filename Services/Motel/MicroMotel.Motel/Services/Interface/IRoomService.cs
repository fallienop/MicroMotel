using MicroMotel.Motel.DTOs.RoomDTOs;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Motel.Services.Interface
{
    public interface IRoomService
    {
        Task<Response<List<RoomDTO>>> GetAllRooms();
        Task<Response<RoomDTO>> GetRoomById(int id);
        Task<Response<NoContent>> CreateNewRoom(RoomCreateDTO rcd);
        Task<Response<NoContent>> UpdateRoom(RoomUpdateDTO rud);
        Task<Response<NoContent>> DeleteRoomById(int id);
    }
}
