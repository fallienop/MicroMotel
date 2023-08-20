// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4;
using MicroMotel.IdentityServer.Data;
using MicroMotel.IdentityServer.Models;
using MicroMotel.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using static IdentityServer4.IdentityServerConstants;
using static System.Net.WebRequestMethods;

namespace MicroMotel.IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
           

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddScoped<ClaimsFactory>();    
            services.AddLocalApiAuthentication();
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlServerIdentity")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders().AddClaimsPrincipalFactory<ClaimsFactory>();
            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            builder.AddResourceOwnerValidator<IdentityServicePasswordValidator>();
            // not recommended for production - you need to store your key material somewhere secure
            builder.AddDeveloperSigningCredential();


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                //options.Authority = "http://localhost:5001"; // IdentityServer's base URL
                //options.RequireHttpsMetadata = false; // For development purposes only, use HTTPS in production
                //options.Audience = LocalApi.ScopeName; // The name of your API resource
                #region problemler
                //options.TokenValidationParameters = new TokenValidationParameters
                //{

                //    RoleClaimType = JwtClaimTypes.Role,

                //}; 
                #endregion

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var claims =  context.Principal.Claims.ToList();

                        // Örnek: JWT içerisindeki "role" claim'ini haritalama
                        var roleClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                        if (roleClaim != null)
                        {
                            var identity = context.Principal.Identity as ClaimsIdentity;
                            identity.AddClaim(new Claim(ClaimTypes.Role, roleClaim.Value));
                        }

                        // Diğer claim haritalamalarını buraya ekleyebilirsiniz
                    }
                };
            }).AddGoogle(options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                // register your IdentityServer with Google at https://console.developers.google.com
                // enable the Google+ API
                // set the redirect URI to https://localhost:5001/signin-google
                options.ClientId = "copy client ID from Google here";
                options.ClientSecret = "copy client secret from Google here";
            }); 
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}