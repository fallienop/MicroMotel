using AutoMapper;
using MicroMotel.Services.Reservation.Context;
using MicroMotel.Services.Reservation.DTOs.MealRDTOs;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MicroMotel.Services.Reservation.Services.Abstract
{
    public class MealRService : IMealRService
    {

        private readonly ReservationContext _context;
        private readonly IMapper _mapper;

        public MealRService(ReservationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<NoContent>> CreateReservation(MealRCreateDTO mealR)
        {
            var meal = _mapper.Map<MealR>(mealR);
            await _context.AddAsync(meal);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("error", 500);

        }

        public async Task<Response<NoContent>> DeleteMealReservation(int id)
        {
           var meal= await _context.FindAsync<MealR>(id);
            if(meal == null)
            {
                return Response<NoContent>.Fail("Not Found", 404);

            }
            _context.Remove(meal);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("error", 500);
        }

        public async Task<Response<List<MealRDTO>>> GetAllMealRs()
        {
            var reservedmeals = await _context.Set<MealR>().ToListAsync();
            var resp=_mapper.Map<List<MealRDTO>>(reservedmeals);
            return Response<List<MealRDTO>>.Success(resp, 200);

        }

        public async Task<Response<List<MealRDTO>>> GetAllMealsByProperty(int propertyid)
        {
            
            var roomsofprop = await _context.RoomReservations.Where(x => x.PropertyId == propertyid).ToListAsync();
            if(roomsofprop == null)
            {
                return Response<List<MealRDTO>>.Fail("Property not found", 404);
            }
            var roomids=roomsofprop.Select(x=>x.Id).ToList();
            var mealsorprop = await _context.MealReservations.Where(x => roomids.Contains(x.RoomRId)).ToListAsync();
            var meals=_mapper.Map<List<MealRDTO>>(mealsorprop);
            return Response<List<MealRDTO>>.Success(meals, 200);
        }

        public async Task<Response<List<MealRDTO>>> GetAllMealsByRoom(int roomid)
        {
           var room=await _context.RoomReservations.Where(x=>x.RoomId == roomid).ToListAsync();
            if (room == null)
            {
                return Response<List<MealRDTO>>.Fail("error", 404);
            }
            var roomrids=room.Select(roomr=>roomr.Id).ToList();
            var mealsofroom =await _context.MealReservations.Where(x=>roomrids.Contains(x.RoomRId)).ToListAsync();
            
            var meals = _mapper.Map<List<MealRDTO>>(mealsofroom);
            return Response<List<MealRDTO>>.Success(meals, 200);

        }

        public async Task<Response<MealRDTO>> GetMealRById(int id)
        {
            var reservedmeal = await _context.FindAsync<MealR>(id);
            var resp = _mapper.Map<MealRDTO>(reservedmeal);
            if (reservedmeal == null)
            {
                return Response<MealRDTO>.Fail("Not Found", 404);
            }
            return Response<MealRDTO>.Success(resp, 200);

        }
       
       
    }
}
