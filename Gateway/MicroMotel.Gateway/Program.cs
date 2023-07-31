using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration.AddJsonFile($"configuration.{builder.Environment.EnvironmentName.ToLower()}.json");
builder.Services.AddAuthentication().AddJwtBearer("GatewayScheme", opt =>
{
    opt.Authority = builder.Configuration["IdentityServer"];
    opt.Audience = "resource_gateway";
    opt.RequireHttpsMetadata = false;
});
builder.Services.AddOcelot();
var app = builder.Build();

await app.UseOcelot();  
    
app.Run();
