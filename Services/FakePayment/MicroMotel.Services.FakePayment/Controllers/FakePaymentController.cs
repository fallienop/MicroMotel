using MicroMotel.Shared.ControllerBases;
using MicroMotel.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroMotel.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomControllerr
    {
        
        [HttpPost] 
        public IActionResult Payment()
        {
            return CustomActionResult(Response<NoContent>.Success(200));
        } 
    }
}
