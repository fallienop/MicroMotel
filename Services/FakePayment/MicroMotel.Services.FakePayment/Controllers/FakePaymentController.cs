using MicroMotel.Services.FakePayment.Context;
using MicroMotel.Services.FakePayment.Models;
using MicroMotel.Shared.ControllerBases;
using MicroMotel.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace MicroMotel.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentController : CustomControllerr
    {
        private readonly CardDbContext _cardDbContext;

        public FakePaymentController(CardDbContext cardDbContext)
        {
            _cardDbContext = cardDbContext;
        }

        [HttpPost] 
        public async Task<IActionResult> Payment(PaymentInputDTO paymentInput)
        {
            bool response=false;
            if (_cardDbContext.Cards.Any(x => x.CardNumber == paymentInput.CardNumber && x.CardExpiration == paymentInput.CardExpiration && x.CVV == paymentInput.CVV)){
              var card=  await _cardDbContext.Cards.Where(x => x.CardNumber == paymentInput.CardNumber).FirstAsync();
         
                card.Balance-=paymentInput.TotalPrice;  
                if(card.Balance < 0)
                {
                    response= false;
                }
               _cardDbContext.Update(card);
               var resp= await _cardDbContext.SaveChangesAsync();
                if (resp > 0)
                {
                    response=true;
                }
                else
                {
                    response = false;   
                }
            }
         
            return CustomActionResult(Response<bool>.Success(response,200));
        }

        [HttpGet("{cardnumber}")]
        public async Task<IActionResult>GetCard(string cardnumber)
        {
           var card=await _cardDbContext.Cards.Where(x=>x.CardNumber==cardnumber).FirstAsync();
            return CustomActionResult(Response<Card>.Success(card,200));    
        }
    }
}
