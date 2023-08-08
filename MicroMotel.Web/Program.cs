using MicroMotel.Shared.Services;
using MicroMotel.Web.Handler;
using MicroMotel.Web.Models.BaseModels;
using MicroMotel.Web.Services.Abstract;
using MicroMotel.Web.Services.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
{
    opt.LoginPath = "/Auth/Signin";
    opt.ExpireTimeSpan = TimeSpan.FromDays(60);
    opt.SlidingExpiration = true;
    opt.Cookie.Name = "micromotelcookie";
});
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
builder.Services.AddHttpClient<IMotelService, MotelService>(opt =>
{
    opt.BaseAddress = new Uri($"{serviceurls.GatewayURL}/{serviceurls.Motel.Path}");
}).AddHttpMessageHandler<CCTokenHandler>();  
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient<IUserService, UserService>(opt =>
{
    opt.BaseAddress = new Uri($"{serviceurls.GatewayURL}/{serviceurls.IdentityServerURL}");
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
