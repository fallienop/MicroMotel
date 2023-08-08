namespace MicroMotel.Web.Models.Motel.Meal
{
    public class MealUpdateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PrepTime { get; set; }
        public int PropertyId { get; set; }

    }
}
