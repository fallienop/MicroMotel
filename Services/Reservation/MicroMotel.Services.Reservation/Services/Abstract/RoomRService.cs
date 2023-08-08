using AutoMapper;
using AutoMapper.Configuration.Conventions;
using MicroMotel.Services.Reservation.Context;
using MicroMotel.Services.Reservation.DTOs.RoomRDTOs;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MicroMotel.Services.Reservation.Services.Abstract
{
    public class RoomRService : IRoomRService
    {
        private readonly ReservationContext _reservationContext;
        private readonly IMapper _mapper;

        public RoomRService(ReservationContext reservationContext, IMapper mapper)
        {
            _reservationContext = reservationContext;
            _mapper = mapper;
        }

        public async Task<Response<NoContent>> CreateReservation(RoomRCreateDTO Roomr)
        {

            var resp = _mapper.Map<RoomR>(Roomr);

            await _reservationContext.AddAsync(resp);
            
            await _reservationContext.SaveChangesAsync();
            return Response<NoContent>.Success(200);
        }

        public async Task<Response<NoContent>> DeleteRoomReservation(int id)
        {
            var room = await _reservationContext.FindAsync<RoomR>(id);
            if (room == null)
            {
                return Response<NoContent>.Fail("error", 200);
            }
            _reservationContext.Remove(room);
            await _reservationContext.SaveChangesAsync();
            return Response<NoContent>.Success(200);    
        }

        public async Task<Response<List<RoomRDTO>>> GetAllRoomRs()
        {
            var listreservedroom = await _reservationContext.Set<RoomR>().Include(r=>r.MealRs).ToListAsync();
            var resp = _mapper.Map<List<RoomRDTO>>(listreservedroom);
            return Response<List<RoomRDTO>>.Success(resp, 200);


        }

        public async Task<Response<List<RoomRDTO>>> GetAllRoomRsbyPropertyId(int propertyid)
        {
            var roomrsofprop = await _reservationContext.Set<RoomR>().Where(x => x.PropertyId == propertyid).Include(x => x.MealRs).ToListAsync();
            if (roomrsofprop == null)
            {
                return Response<List<RoomRDTO>>.Fail("not found", 404);

            }
            var resp = _mapper.Map<List<RoomRDTO>>(roomrsofprop);
            return Response<List<RoomRDTO>>.Success(resp, 200);
            
        }

        public async Task<Response<List<RoomRDTO>>> GetAllRoomRsbyRoomId(int roomid)
        {
            var roomrsofprop = await _reservationContext.Set<RoomR>().Where(x => x.RoomId == roomid).Include(x => x.MealRs).ToListAsync();
            if (roomrsofprop == null)
            {
                return Response<List<RoomRDTO>>.Fail("not found", 404);

            }
            var resp = _mapper.Map<List<RoomRDTO>>(roomrsofprop);
            return Response<List<RoomRDTO>>.Success(resp, 200);

        }
        public async Task<Response<RoomRDTO>> GetRoomRById(int id)
        {
            var reservedroom = await _reservationContext.Set<RoomR>().FindAsync(id);
            var resp=_mapper.Map<RoomRDTO>(reservedroom);   
            if (reservedroom == null)
            {
                return Response<RoomRDTO>.Fail("Not Found", 404);
            }
            return Response<RoomRDTO>.Success(resp, 200);
        }

       
       
    }
}
