    using MicroMotel.Motel.DTOs.PropertyDTOs;
    using MicroMotel.Motel.DTOs.RoomDTOs;
    using MicroMotel.Services.Motel.Services.Abstract;
    using MicroMotel.Services.Motel.Services.Interface;
    using MicroMotel.Shared.ControllerBases;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    namespace MicroMotel.Services.Motel.Controllers
    {
        [Route("api/[controller]")]
        [ApiController]
        public class RoomController : CustomControllerr
        {
            private readonly IRoomService _roomservice;

            public RoomController(IRoomService roomservice)
            {
                _roomservice = roomservice;
            }

            [HttpGet]
            public async Task<IActionResult> GetAll()
            {
                var resp = await _roomservice.GetAllRooms();
                return CustomActionResult(resp);
            }
            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var resp = await _roomservice.GetRoomById(id);
                return CustomActionResult(resp);
            }

            [HttpPost]
            public async Task<IActionResult> NewProp([FromBody] RoomCreateDTO rcd)
            {
                var resp = await _roomservice.CreateNewRoom(rcd);
                return CustomActionResult(resp);
            }

            [HttpPut]
            public async Task<IActionResult> Update(RoomUpdateDTO rud)
            {
                var resp = await _roomservice.UpdateRoom(rud);
                return CustomActionResult(resp);
            }


            [Route("Delete/{id}")]     
            [HttpDelete]
            public async Task<IActionResult> Delete(int id)
            {
                var resp = await _roomservice.DeleteRoomById(id);
                return CustomActionResult(resp);

            }

            [HttpGet("combined/{id}")]
            public async Task<IActionResult> getWithRooms(int id)
            {
          
                var resp = await _roomservice.GetWithRooms(id);
                return CustomActionResult(resp);
            }

    }
}
