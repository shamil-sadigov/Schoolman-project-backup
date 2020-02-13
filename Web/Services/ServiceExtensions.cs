using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Services
{
    public static class ServiceExtensions
    {
        public static void AddDefaultCors(this IServiceCollection services)
        {
            services.AddCors(ops =>
            {
                ops.AddPolicy("Default", corsOps => corsOps.AllowAnyOrigin()
                                                              .AllowAnyMethod()
                                                              .AllowAnyHeader()
                                                              .Build());
            });
        }




        public static void UseDefaultCors(this IApplicationBuilder app)
        {
            app.UseCors("Default");
        }


        public static void AddSwagger(this IServiceCollection services)
        {
            var assemblyName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml");
            var pathToAssembly = Path.Combine(AppContext.BaseDirectory, assemblyName);

            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Schoolman - WebAPI",
                    Version = "v1"

                });
                ops.IncludeXmlComments(pathToAssembly);
                ops.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Scheme = "Bearer"
                });

                ops.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                     {
                         new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                },
                                    Scheme = "Bearer",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                         }, new List<string>()
                     }
                });
            });
        }
    }
}
