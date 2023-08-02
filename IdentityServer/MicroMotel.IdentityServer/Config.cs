// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MicroMotel.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
        {
            new ApiResource("resource_motel"){Scopes={"motel_fullpermission" } },
            new ApiResource("resource_photo"){Scopes={"photo_fullpermission"}},
            new ApiResource("resource_payment"){Scopes={"payment_fullpermission"}}, 
            new ApiResource("resource_reservation"){Scopes={ "reservation_fullpermission"} },
            new ApiResource("resource_gateway"){Scopes={"gateway_fullpermission" } },
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        };
        public static IEnumerable<IdentityResource> IdentityResources =>
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource(){Name="Roles",DisplayName="Roles",Description="User roles",UserClaims= new[]{"Roles"}}
                   };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
               new ApiScope("motel_fullpermission","full permission for motel"),
               new ApiScope("photo_fullpermission","full permission for photostock"),
               new ApiScope("payment_fullpermission","full permission for payment"),
               new ApiScope("reservation_fullpermission","full permission for reservation"),
               new ApiScope("gateway_fullpermission","full permission for gateway"),
               new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                
                new Client
                {
                    ClientName="for non-registered users",
                    ClientSecrets={new Secret("nonregister".Sha512()) },
                   ClientId="nonregisterd",
                   AllowedGrantTypes=GrantTypes.ClientCredentials,
                    AllowedScopes={IdentityServerConstants.LocalApi.ScopeName, "gateway_fullpermission", "motel_fullpermission", "photo_fullpermission" } 
                } ,
                new Client
                {
                    ClientName="for registered users",
                    ClientId="forregistered",
                    ClientSecrets={new Secret ("register".Sha512()) },
                    AllowedGrantTypes=GrantTypes.ResourceOwnerPassword,
                    AllowedScopes={IdentityServerConstants.StandardScopes.OfflineAccess,IdentityServerConstants.LocalApi.ScopeName,IdentityServerConstants.StandardScopes.Profile,IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId, "payment_fullpermission", "reservation_fullpermission", "gateway_fullpermission"},
                    AllowOfflineAccess=true,
                    AccessTokenLifetime=3600,
                    RefreshTokenExpiration=TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime=(int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                        RefreshTokenUsage=TokenUsage.ReUse

                }
            };
    }
}