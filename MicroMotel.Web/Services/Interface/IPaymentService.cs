using MicroMotel.Web.Models.FakePayment;

namespace MicroMotel.Web.Services.Interface
{
    public interface IPaymentService
    {
        Task<bool> ReceivePayment(PaymentInput paymentInput);

    }
}
