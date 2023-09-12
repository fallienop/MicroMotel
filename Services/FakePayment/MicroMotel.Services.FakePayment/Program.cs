using MicroMotel.Services.FakePayment.Context;
using MicroMotel.Services.FakePayment.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
var requireauthenticateduser=new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
builder.Services.AddAuthentication().AddJwtBearer(opt =>
{
    opt.Audience = "resource_payment";
    opt.Authority = builder.Configuration["IdentityServerUrl"];
    opt.RequireHttpsMetadata = false;
});

builder.Services.AddControllers(opt =>
{
   opt.Filters.Add(new AuthorizeFilter(requireauthenticateduser));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CardDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
var app = builder.Build();
using(var scope = app.Services.CreateScope())
{
    var service=scope.ServiceProvider;
    var context=service.GetRequiredService<CardDbContext>();
    context.Database.Migrate();
    if (!context.Cards.Any())
    {
        var card = new Card { Email = "ulvi.babayev.200@gmail.com",CardExpiration= "02/26",CardNumber= "1020304010203040",CVV=200,Owner= "Shahin",Balance=9999999 };
        context.Cards.Add(card);
        context.SaveChanges();
    }
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
