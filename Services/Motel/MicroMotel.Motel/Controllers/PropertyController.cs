using MicroMotel.Motel.DTOs.PropertyDTOs;
using MicroMotel.Services.Motel.Services.Interface;
using MicroMotel.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MicroMotel.Services.Motel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : CustomControllerr
    {
        private readonly IPropertyService _propertyservice;

        public PropertyController(IPropertyService propertyservice)
        {
            _propertyservice = propertyservice;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _propertyservice.GetAllProperties();
            return CustomActionResult(resp);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resp=await _propertyservice.GetPropertyById(id);
            return CustomActionResult(resp);
        }

        [HttpPost]
        public async Task<IActionResult> NewProp([FromBody]PropertyCreateDTO pcd)
        {
            var resp = await _propertyservice.CreateNewProperty(pcd);
            return CustomActionResult(resp);
        }

        [HttpPut]
        public async Task<IActionResult> Update(PropertyUpdateDTO pud)
        {
            var resp=await _propertyservice.UpdateProperty(pud);
            return CustomActionResult(resp);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var resp=await _propertyservice.DeletePropertyById(id);
            return CustomActionResult(resp);

        }
   

    }
}
