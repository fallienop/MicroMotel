namespace MicroMotel.Web.Models.FakePayment
{
    public class PaymentInput
    {
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public int CVV { get; set; }
        public decimal TotalPrice { get; set; }
        public string Owner { get; set; }
    }
}
