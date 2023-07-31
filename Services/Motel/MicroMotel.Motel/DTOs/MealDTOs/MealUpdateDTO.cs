namespace MicroMotel.Services.Motel.DTOs.MealDTOs
{
    public class MealUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PrepTime { get; set; }
    }
}
