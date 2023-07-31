using Microsoft.EntityFrameworkCore;
using MicroMotel.Motel.Context;
using MicroMotel.Shared.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MicroMotel.Services.Motel.Services.Interface;
using MicroMotel.Services.Motel.Services.Abstract;
using MicroMotel.Motel.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddControllers(opt=>opt.Filters.Add(new AuthorizeFilter()));
builder.Services.AddScoped<IPropertyService,PropertyService>();
builder.Services.AddScoped<IRoomService,RoomService>();
builder.Services.AddScoped<IMealService,MealService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.Audience = "resource_motel";
    opt.RequireHttpsMetadata = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MotelContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerOrder" )));
var app = builder.Build();
using(var scope=app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<MotelContext>();

    if (!context.Properties.Any())
    {
        var address = new Address { City = "Baku", District = "kyrdakhani", Street = "dadas vasif", Building = "15a" };
       

        context.Properties.Add(new Property {  Address = address, HasOpenSpace = true, HasParking = true, FloorCount = 2, Name = "qarayevmicromotel", RoomCount = 15 });

        context.SaveChanges();
    }
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
