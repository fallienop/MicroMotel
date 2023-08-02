using MicroMotel.Services.Reservation.Models;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.ControllerBases;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Services.Reservation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomRController : CustomControllerr
    {
        private readonly IRoomRService _roomRService;

        public RoomRController(IRoomRService roomRService)
        {
            _roomRService = roomRService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var resp = await _roomRService.GetAllRoomRs();
            return CustomActionResult(resp);
        }

        [HttpGet("{id}")]
        
        public async Task<IActionResult> GetById(int id)
        {
            var resp=await _roomRService.GetRoomRById(id);
            return CustomActionResult(resp);
        }

        [HttpPost]
        public async Task<IActionResult> newres(RoomR roomr)
        {
            var resp = await _roomRService.CreateReservation(roomr);
            return CustomActionResult(resp);

        }
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _roomRService.DeleteRoomReservation(id);
            return CustomActionResult(res);
        }

        [HttpPut]
        public async Task<IActionResult> Update(RoomR roomr)
        {
            var res = await _roomRService.UpdateReservation(roomr);
            return CustomActionResult(res);
        }
    }
}
