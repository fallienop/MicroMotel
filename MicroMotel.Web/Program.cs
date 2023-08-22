using MicroMotel.Shared.Services;
using MicroMotel.Web.Handler;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using FluentValidation.AspNetCore;
using MicroMotel.Web.Models.Reservation.RoomR;
using MicroMotel.Web.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using MicroMotel.Web.Helpers;

var builder = WebApplication.CreateBuilder(args);
var requriedauthorizepolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Auth/Signin"; 
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
    opt.AccessDeniedPath = "/AnotherPage/unauthorized";
    opt.Cookie.Name = "micromotelcookie";
});
builder.Services.AddHttpContextAccessor();
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
builder.Services.AddAccessTokenManagement();    
var serviceurls = builder.Configuration.GetSection("ServiceURLs").Get<ServiceURLs>();

builder.Services.Configure<ClientSettings>(builder.Configuration.GetSection("ClientSettings"));
builder.Services.Configure<ServiceURLs>(builder.Configuration.GetSection("ServiceURLs"));
builder.Services.AddScoped<ROPTokenHandler>();
builder.Services.AddScoped<CCTokenHandler>();
builder.Services.AddHttpClient<ICCService, CCService>();
builder.Services.AddHttpClient<IROPService, ROPService>();
builder.Services.AddScoped<ISharedIdentityService, SharedIdentityService>();
builder.Services.AddSingleton<PhotoHelper>();
builder.Services.AddHttpClient<IReservationService, ReservationService>(opt => 
{
    opt.BaseAddress = new Uri($"{serviceurls.GatewayURL}/{serviceurls.Reservation.Path}");
}).AddHttpMessageHandler<ROPTokenHandler>();

builder.Services.AddHttpClient<IPhotoStockService, PhotoStockService>(opt=>
{
    opt.BaseAddress = new Uri($"{serviceurls.GatewayURL}/{serviceurls.Photos.Path}");
}).AddHttpMessageHandler<CCTokenHandler>();

builder.Services.AddHttpClient<IMotelService, MotelService>(opt =>
{
    opt.BaseAddress = new Uri($"{serviceurls.GatewayURL}/{serviceurls.Motel.Path}");
}).AddHttpMessageHandler<CCTokenHandler>();

// Add services to the container.
builder.Services.AddHttpClient<ISignUpService, SignUpService>(opt =>
{
    opt.BaseAddress = new Uri($"{serviceurls.IdentityServerURL}");
}).AddHttpMessageHandler<CCTokenHandler>();
//builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters() ;
builder.Services.AddControllersWithViews(opt =>
{
    opt.Filters.Add(new AuthorizeFilter(requriedauthorizepolicy));
});

builder.Services.AddHttpClient<IUserService, UserService>(opt =>
{
    opt.BaseAddress = new Uri($"{serviceurls.IdentityServerURL}");
}).AddHttpMessageHandler<ROPTokenHandler>();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute(
        name: "admin",
        pattern: "admin",
        defaults: new { controller = "Admin", action = "PropertyList" }
    );

    endpoints.MapControllerRoute(name: "getuser", pattern: "getallusers", defaults: new {controller="admin",action= "GetAllUsers" });
});
app.Run();
