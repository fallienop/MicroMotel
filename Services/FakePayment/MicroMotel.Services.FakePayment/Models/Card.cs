namespace MicroMotel.Services.FakePayment.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string CardExpiration { get; set; }
        public int CVV { get; set; }
        public decimal Balance { get; set; }
        public string Owner { get; set; }   
        public string Email { get; set; }

    }
}
