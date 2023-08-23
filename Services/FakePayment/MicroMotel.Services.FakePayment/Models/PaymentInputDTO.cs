namespace MicroMotel.Services.FakePayment.Models
{
    public class PaymentInputDTO
    {
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public int CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public string Owner { get; set; }
    }
}
