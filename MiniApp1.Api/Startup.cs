using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Sharedlayer.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sharedlayer.Extensions;
using MiniApp1.Api.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CustomTokenOptions>(Configuration.GetSection("TokenOption"));
            var tokenoptions = Configuration.GetSection("TokenOption").Get<CustomTokenOptions>();

            services.JwtBearerConfiguration(tokenoptions);
            services.AddSingleton<IAuthorizationHandler, BirthDayRequirementHandler>();
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("AmasyaPolicy", policy =>
                {
                    policy.RequireClaim("city", "amasya");
                });

                opt.AddPolicy("AgePolicy", policy =>
                {
                    policy.Requirements.Add(new BirthDayRequirement(18));
                });
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniApp1.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MiniApp1.Api v1"));
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
