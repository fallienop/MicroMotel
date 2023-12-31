﻿using AutoMapper;
using MicroMotel.Motel.Context;
using MicroMotel.Motel.DTOs.PropertyDTOs;
using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.DTOs.PropertyDTOs;
using MicroMotel.Services.Motel.Services.Interface;
using MicroMotel.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MicroMotel.Services.Motel.Services.Abstract
{
    public class PropertyService : IPropertyService
    {
        private readonly MotelContext _context;
        private readonly IMapper _mapper;

        public PropertyService(MotelContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Response<int>> CreateNewProperty(PropertyCreateDTO pcd)
        {
            var prop = _mapper.Map<Property>(pcd);
            var response = await _context.AddAsync(prop);
            var booll = await _context.SaveChangesAsync();
            var id = prop.Id;
            return Response<int>.Success(id, 200);
        }
   
        public async Task<Response<NoContent>> DeletePropertyById(int id)
        {
            var p  = await _context.FindAsync<Property>(id);
            if (p == null)
            {
                return Response<NoContent>.Fail("not found", 404);
            }
             _context.Remove(p);
          var booll=  await _context.SaveChangesAsync();
            if (booll > 0)
            {
                return Response<NoContent>.Success(200);
            }
            else
            {
                return Response<NoContent>.Fail("cannot deleted", 500);
            }
        }

        public async Task<Response<List<PropertyDTO>>> GetAllProperties()
        {
           

            var properties = await _context.Properties.ToListAsync();
            var propertyDTOs = _mapper.Map<List<PropertyDTO>>(properties);
            return Response<List<PropertyDTO>>.Success(propertyDTOs, 200);
        }

        public async Task<Response<PropertyDTO>> GetPropertyById(int id)
        {
            var prop= await _context.Set<Property>().FindAsync(id);
            if(prop == null)
            {
                return Response<PropertyDTO>.Fail("not found", 404);
            }
            var response= _mapper.Map<PropertyDTO>(prop);
            return Response<PropertyDTO>.Success(response, 200);

        }

          

        public async Task<Response<NoContent>> UpdateProperty(PropertyUpdateDTO pud)
        {
            var prop = _mapper.Map<Property>(pud);
          _context.Update(prop);
          var r=  _context.SaveChangesAsync();
            if (r.Result > 0)
            {
            return Response<NoContent>.Success(200);

            }
            return Response<NoContent>.Fail("error",400);

        }


    }
}
