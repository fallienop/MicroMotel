namespace MicroMotel.Web.Models.Motel.Meal
{
    public class MealCreateInput
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int PrepTime { get; set; }
        public int PropertyId { get; set; }

    }
}
