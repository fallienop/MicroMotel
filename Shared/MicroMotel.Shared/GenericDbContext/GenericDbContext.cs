//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection.Emit;
//using System.Text;
//using System.Threading.Tasks;

//namespace MicroMotel.Shared.GenericDbContext;
//public class GenericDbContext<TContext> : DbContext where TContext : DbContext
//{
//    protected GenericDbContext(DbContextOptions<TContext> options) : base(options)
//    {

//    }

//    public DbSet<T> Set<T>() where T : class
//    {
//        return base.Set<T>();
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder)
//    {
//        base.OnModelCreating(modelBuilder);
//    }

//    public async Task<List<TEntity>> GetAll<TEntity>() where TEntity : class
//    {
//        return await Set<TEntity>().ToListAsync();
//    }

//    public async Task<TEntity> GetById<TEntity>(int id) where TEntity : class
//    {
//        return await Set<TEntity>().FindAsync(id);
//    }

//    public async Task Add<TEntity>(TEntity entity) where TEntity : class
//    {
//        await Set<TEntity>().AddAsync(entity);
//        await SaveChangesAsync();
//    }

//    public async Task Update<TEntity>(TEntity entity) where TEntity : class
//    {
//        Set<TEntity>().Update(entity);
//        await SaveChangesAsync();
//    }

//    public async Task Delete<TEntity>(int id) where TEntity : class
//    {
//        var entity = await Set<TEntity>().FindAsync(id);
//        if (entity != null)
//        {
//            Set<TEntity>().Remove(entity);
//            await SaveChangesAsync();
//        }
//    }
//}