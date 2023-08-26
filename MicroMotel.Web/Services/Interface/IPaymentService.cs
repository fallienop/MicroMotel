using MicroMotel.Web.Models.FakePayment;

namespace MicroMotel.Web.Services.Interface
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInput paymentInput);
        Task<Card> GetCard(string cardnumber);
        Task<bool> TestCard(PaymentInput paymentInput);
    }
}
