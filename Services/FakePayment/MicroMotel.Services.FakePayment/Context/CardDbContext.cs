using MicroMotel.Services.FakePayment.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroMotel.Services.FakePayment.Context
{
    public class CardDbContext:DbContext
    {
        public string Schema = "Card";

        public CardDbContext(DbContextOptions<CardDbContext> options) : base(options)
        {
        }
        public DbSet<Card> Cards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>().ToTable("Card", Schema);

            modelBuilder.Entity<Card>().Property(x => x.Balance).HasColumnType("decimal(18,2)");
            base.OnModelCreating(modelBuilder);
        }
    }
}
