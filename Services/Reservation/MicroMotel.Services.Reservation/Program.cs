using MicroMotel.Services.Reservation.Context;
using MicroMotel.Services.Reservation.Services.Abstract;
using MicroMotel.Services.Reservation.Services.Interface;
using MicroMotel.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
var requriedauthorizepolicy=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
builder.Services.AddAuthentication().AddJwtBearer(opt => 
{
    opt.Audience = "resource_reservation";
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.RequireHttpsMetadata = false;
});
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requriedauthorizepolicy));
}) ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRoomRService,RoomRService>();
builder.Services.AddScoped<IMealRService,MealRService>();
builder.Services.AddDbContext<ReservationContext>(opt=>opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerOrder")));
var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context= service.GetRequiredService<ReservationContext>();  
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
