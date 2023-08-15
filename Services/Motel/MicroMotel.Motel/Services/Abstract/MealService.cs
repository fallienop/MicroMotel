using AutoMapper;
using MicroMotel.Motel.Context;
using MicroMotel.Services.Motel.DTOs.MealDTOs;
using MicroMotel.Services.Motel.Models;
using MicroMotel.Services.Motel.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MicroMotel.Services.Motel.Services.Abstract
{
    public class MealService : IMealService
    {
        private readonly MotelContext _context;
        private readonly IMapper _mapper;

        public MealService(MotelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<NoContent>> CreateNewMeal(MealCreateDTO mcd)
        {
            var meal = _mapper.Map<Meal>(mcd);
            await _context.AddAsync(meal);
            var r = _context.SaveChangesAsync();
            if (r.Result > 0)
            {
                return Response<NoContent>.Success(200);

            }
            return Response<NoContent>.Fail("error", 400);

        }

        public async Task<Response<NoContent>> DeleteMealById(int id)
        {
           var meal=await _context.FindAsync<Meal>(id);
            if(meal == null)
            {
                return Response<NoContent>.Fail("not found", 404);
            }
            _context.Remove(meal);
            var r = await _context.SaveChangesAsync();
            if (r> 0)
            {
                return Response<NoContent>.Success(200);

            }
           else { return Response<NoContent>.Fail("error", 500);}

        }

        public async Task<Response<List<MealDTO>>> GetAllMeals(int propertyid)
        {
            var meals=await _context.Meals.Where(x=>x.PropertyId==propertyid).ToListAsync();
            var resp=_mapper.Map<List<MealDTO>>(meals);

            return Response<List<MealDTO>>.Success(resp, 200);
        }


        public async Task<Response<MealDTO>> GetMealById(int id)
        {
            var meal = await _context.Meals.FindAsync(id);
            if(meal == null)
            {
                return Response<MealDTO>.Fail("not found", 404);

            }
            var resp=_mapper.Map<MealDTO>(meal);
            return Response<MealDTO>.Success(resp, 200);
            
        }

        public async Task<Response<NoContent>> UpdateMeal(MealUpdateDTO mud)
        {
            var updmeal = _mapper.Map<Meal>(mud);
            _context.Update(updmeal);
            var r = _context.SaveChangesAsync();
            if (r.Result > 0)
            {
                return Response<NoContent>.Success(200);

            }
            return Response<NoContent>.Fail("error", 400);
        }
    }
}
