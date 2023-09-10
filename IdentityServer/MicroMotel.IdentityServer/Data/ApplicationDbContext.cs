using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MicroMotel.IdentityServer.Models;

namespace MicroMotel.IdentityServer.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(e=>e.UserName).IsUnique();
                entity.HasIndex(e=>e.Email).IsUnique();
                entity.Property(e => e.Budget)
                           .HasColumnType("decimal(18, 2)");
            });
     
           
        }
    }
}
