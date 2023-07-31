using MicroMotel.Services.Reservation.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MicroMotel.Services.Reservation.Context
{
    public class ReservationContext:DbContext
    {
        public const string Schema = "Reservation";

        public ReservationContext(DbContextOptions<ReservationContext> options) : base(options)
        {
        }
        public DbSet<RoomR> RoomReservations { get; set; }
        public DbSet<MealR> MealReservations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoomR>().ToTable("Rooms",Schema);
            modelBuilder.Entity<MealR>().ToTable("Meals",Schema);
          
            base.OnModelCreating(modelBuilder);
        }
    }
}
