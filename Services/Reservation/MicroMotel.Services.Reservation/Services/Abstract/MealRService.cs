using MicroMotel.Services.Reservation.Context;
using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MicroMotel.Services.Reservation.Services.Abstract
{
    public class MealRService : IMealRService
    {

        private readonly ReservationContext _context;

        public MealRService(ReservationContext context)
        {
            _context = context;
        }

        public async Task<Response<NoContent>> CreateReservation(MealR MealR)
        {
            await _context.AddAsync(MealR);
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

        public async Task<Response<List<MealR>>> GetAllMealRs()
        {
            var reservedmeals = await _context.Set<MealR>().ToListAsync();
            return Response<List<MealR>>.Success(reservedmeals, 200);

        }

        public async Task<Response<MealR>> GetMealRById(int id)
        {
            var reservedmeal = await _context.FindAsync<MealR>(id);
            if (reservedmeal == null)
            {
                return Response<MealR>.Fail("Not Found", 404);
            }
            return Response<MealR>.Success(reservedmeal, 200);

        }

        public async Task<Response<NoContent>> UpdateReservation(MealR MealR)
        {
             _context.Update(MealR);
            var res = await _context.SaveChangesAsync();
            if (res > 0)
            {
                return Response<NoContent>.Success(200);
            }
            return Response<NoContent>.Fail("error", 500);
        }
    }
}
