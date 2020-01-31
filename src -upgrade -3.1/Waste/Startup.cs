using Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Web
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
            services.AddCors(ops =>
            {
                ops.AddPolicy("Default", corsOps => corsOps.AllowAnyOrigin()
                                                              .AllowAnyMethod()
                                                              .AllowAnyHeader()
                                                              .AllowCredentials()
                                                              .Build());
            });

            #region Swagger Configuration

            var assemblyName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml");
            var pathToAssembly = Path.Combine(AppContext.BaseDirectory, assemblyName);

            services.AddSwaggerGen(ops =>
            {
                ops.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Schoolman.Student - WebAPI",
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

            #endregion

            services.AddPersistenceLayer();
            services.AddAuthenticationLayer();
            services.AddBusinessLayer();

            services.AddControllers();
        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("EnableCORS");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(ops =>
            {
                ops.SwaggerEndpoint("/swagger/v1/swagger.json", "Schoolman.Student.WebAPI v1");
                ops.RoutePrefix = "";
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
