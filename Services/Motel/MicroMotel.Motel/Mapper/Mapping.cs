using AutoMapper;
using MicroMotel.Motel.DTOs.PropertyDTOs;
using MicroMotel.Motel.DTOs.RoomDTOs;
using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.DTOs.MealDTOs;
using MicroMotel.Services.Motel.DTOs.PropertyDTOs;
using MicroMotel.Services.Motel.Models;

namespace MicroMotel.Motel.Mapper
{
    public class Mapping:Profile
    {
        public Mapping()
        {
            CreateMap<Property,PropertyCreateDTO>().ReverseMap();
            CreateMap<Property,PropertyUpdateDTO>().ReverseMap();
            CreateMap<Property, PropertyDTO>().ReverseMap();
            CreateMap<Property,PropertyWithRoomsDTO>().ReverseMap();

            CreateMap<Room,RoomDTO>().ReverseMap();
            CreateMap<Room,RoomCreateDTO>().ReverseMap();
            CreateMap<Room,RoomUpdateDTO>().ReverseMap();

            CreateMap<Meal,MealDTO>().ReverseMap();
            CreateMap<Meal,MealCreateDTO>().ReverseMap();
            CreateMap<Meal,MealUpdateDTO>().ReverseMap();

            
        }
    }
}
