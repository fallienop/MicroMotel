using AutoMapper;
using MicroMotel.Services.Reservation.DTOs.MealRDTOs;
using MicroMotel.Services.Reservation.DTOs.RoomRDTOs;
using MicroMotel.Services.Reservation.Models;
using System.Runtime.InteropServices;

namespace MicroMotel.Services.Reservation.Mapper
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap < RoomR, RoomRDTO       > ().ForMember(dest => dest.MealRs, opt => opt.MapFrom(src => src.MealRs)).ReverseMap();
            CreateMap < MealR, MealRDTO       > ().ReverseMap();
            CreateMap < RoomR, RoomRCreateDTO > ().ReverseMap();
            CreateMap < MealR, MealRCreateDTO > ().ReverseMap();
        



        }
    }
}
