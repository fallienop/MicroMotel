using MicroMotel.Services.Reservation.DTOs.RoomRDTOs;
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

        [HttpGet("prop/{propid}")]
        public async Task<IActionResult> GetRoomrByPropid(int propid)
        {
            var resp=await _roomRService.GetAllRoomRsbyPropertyId(propid);
            return CustomActionResult(resp);
        } 
        
        [HttpGet("room/{roomid}")]
        public async Task<IActionResult> GetRoomrByRoomid(int roomid)
        {
            var resp=await _roomRService.GetAllRoomRsbyRoomId(roomid);
            return CustomActionResult(resp);
        }

        [HttpGet("user/{userid}")]
        public async Task<IActionResult> GetRoomrByUserid(string userid)
        {
            var resp = await _roomRService.GetAllRoomRsbyUserId(userid);
            return CustomActionResult(resp);
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
        public async Task<IActionResult> newres(RoomRCreateDTO roomr)
        {
            var resp = await _roomRService.CreateReservation(roomr);
            return CustomActionResult(resp);

        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var res = await _roomRService.DeleteRoomReservation(id);
            return CustomActionResult(res);
        }

   
    }
}
