using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Extensions;
using Authentication;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence;
using Schoolman.Student.Core.Application.Interfaces;
using WebApi.Services;

namespace WebApi
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
            services.AddApplicationLayer();
            services.AddAuthenticationLayer();
            services.AddBusinessLayer();
            services.AddPersistenceLayer();

            services.AddControllers()
                    .AddFluentValidation(ops =>
                    {
                        ops.RegisterValidatorsFromAssemblyContaining<IUserService>();
                        ops.ImplicitlyValidateChildProperties = false;
                        ops.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    });


            services.AddSwagger();      // app extension method
            services.AddDefaultCors();  // app extension method

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseDefaultCors(); // app extension method

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

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
