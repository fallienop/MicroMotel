using MicroMotel.Motel.Models;
using MicroMotel.Services.Motel.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroMotel.Motel.Context
{
    public class MotelContext:DbContext
    {
        public const string Schema = "Motel";

        public MotelContext(DbContextOptions<MotelContext> options) : base(options)
        {

        }
        public DbSet<Property> Properties { get; set; } 
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Meal> Meals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().ToTable("Room", Schema);
            modelBuilder.Entity<Property>().ToTable(nameof(Property),Schema);
            modelBuilder.Entity<Meal>().ToTable(nameof(Meal),Schema);
            modelBuilder.Entity<Room>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Meal>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Property>().OwnsOne(x => x.Address).WithOwner();
            base.OnModelCreating(modelBuilder);
        }
    }
}
