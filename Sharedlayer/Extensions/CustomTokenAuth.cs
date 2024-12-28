using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Sharedlayer.Configurations;
using Sharedlayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sharedlayer.Extensions
{
    public static class CustomTokenAuth
    {
        public static void JwtBearerConfiguration(this IServiceCollection services, CustomTokenOptions tokenoptions)
        {
            
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//benim authenticetionum bir jwt kullanacağını anlaması lazım bundan dolayı JwtBearerDefaults.AuthenticationScheme veriyorum

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenoptions.Issuer,
                    ValidAudience = tokenoptions.Audience[0],
                    IssuerSigningKey = SignService.GetSymetricSecurityKey(tokenoptions.SecurityKey),

                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero//sen mesela tokena 1 saat verdin kendi default 5 dk daha ekliyo farklı serverlara kurduğum apimin aradaki farkı tölere etmek için 

                };
            });
        }
    }
}
