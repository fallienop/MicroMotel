using AutoMapper.Configuration.Conventions;
using MicroMotel.Services.Reservation.Context;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MicroMotel.Services.Reservation.Services.Abstract
{
    public class RoomRService : IRoomRService
    {
        private readonly ReservationContext _reservationContext;

        public RoomRService(ReservationContext reservationContext)
        {
            _reservationContext = reservationContext;
        }

        public async Task<Response<NoContent>> CreateReservation(RoomR roomR)
        {
            await _reservationContext.AddAsync(roomR);
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
            _reservationContext.SaveChangesAsync();
            return Response<NoContent>.Success(200);    
        }

        public async Task<Response<List<RoomR>>> GetAllRoomRs()
        {
            var listreservedroom = await _reservationContext.Set<RoomR>().ToListAsync();
            return Response<List<RoomR>>.Success(listreservedroom, 200);


        }

        public async Task<Response<RoomR>> GetRoomRById(int id)
        {
            var reservedroom = await _reservationContext.Set<RoomR>().FindAsync(id);
            if (reservedroom == null)
            {
                return Response<RoomR>.Fail("Not Found", 404);
            }
            return Response<RoomR>.Success(reservedroom, 200);
        }

        public async Task<Response<NoContent>> UpdateReservation(RoomR roomR)
        {
            _reservationContext.Update(roomR);
           var res= await _reservationContext.SaveChangesAsync();
            if (res > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("error", 500);
        }
    }
}
