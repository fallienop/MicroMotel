using AutoMapper;
using MicroMotel.Motel.Context;
using MicroMotel.Motel.DTOs.RoomDTOs;
using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MicroMotel.Services.Motel.Services.Abstract
{
    public class RoomService : IRoomService
    {
        private readonly MotelContext _motelContext;
        private readonly IMapper _mapper;

        public RoomService(MotelContext motelContext, IMapper mapper)
        {
            _motelContext = motelContext;
            _mapper = mapper;
        }

        public async Task<Response<NoContent>> CreateNewRoom(RoomCreateDTO rcd)
        {
            var room = _mapper.Map<Room>(rcd);
            await _motelContext.AddAsync(room);
            var r= await _motelContext.SaveChangesAsync();
            if ( r>0 ) { return Response<NoContent>.Success(200); }
            return Response<NoContent>.Fail("error",500);
        }

        public async Task<Response<NoContent>> DeleteRoomById(int id)
        {
            
            var room = await _motelContext.FindAsync<Room>(id);
            if(room == null )
            {
                return Response<NoContent>.Fail("not found", 404);
            }
            _motelContext.Remove(room);
            var rr = await _motelContext.SaveChangesAsync();
            if ( rr > 0 ) { return Response<NoContent>.Success(200); }
            return Response<NoContent>.Fail("error", 500);
        }

        public async Task<Response<List<RoomDTO>>> GetAllRooms()
        { 
            var rooms=await _motelContext.Set<Room>().ToListAsync();
            var response = _mapper.Map<List<RoomDTO>>(rooms);
            return Response<List<RoomDTO>>.Success(response, 200);
        }

    

        public async Task<Response<RoomDTO>> GetRoomById(int id)
        {
            var room = await _motelContext.Set<Room>().FindAsync(id);
            var response=_mapper.Map<RoomDTO>(room);
            if (response == null)
            {
                return Response<RoomDTO>.Fail("error",404);
            }
            return Response<RoomDTO>.Success(response, 200);

        }

        public async Task<Response<NoContent>> UpdateRoom(RoomUpdateDTO rud)
        {
            var roomupd = _mapper.Map<Room>(rud);
            _motelContext.Update(roomupd);
            var r = await _motelContext.SaveChangesAsync();
            if ( r > 0 ) { return Response<NoContent>.Success(200); }
            return Response<NoContent>.Fail("error",500);
        }
    }
}
