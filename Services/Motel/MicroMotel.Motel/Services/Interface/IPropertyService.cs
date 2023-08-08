using MicroMotel.Motel.DTOs.PropertyDTOs;
using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.DTOs.PropertyDTOs;
using MicroMotel.Shared.DTOs;

namespace MicroMotel.Services.Motel.Services.Interface
{
    public interface IPropertyService
    {
        Task<Response<List<PropertyDTO>>> GetAllProperties();
        Task<Response<PropertyDTO>> GetPropertyById(int id);
        Task<Response<NoContent>> CreateNewProperty(PropertyCreateDTO pcd);
        Task<Response<NoContent>> UpdateProperty(PropertyUpdateDTO pud);
        Task<Response<NoContent>> DeletePropertyById(int id);
        Task<Response<PropertyWithRoomsDTO>> GetWithRooms(int id);

        
        
    }
}
