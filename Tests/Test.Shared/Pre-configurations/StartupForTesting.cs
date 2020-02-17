using Application.Extensions;
using Authentication;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Schoolman.Student.Core.Application.Interfaces;

namespace Test.Shared
{
    public class StartupForTesting
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationLayerForTesting();
            services.AddAuthenticationLayerForTesting();
            services.AddBusinessLayerForTesting();
            services.AddPersistenceLayerForTesting();

            // Actually we no need Controller here BUT without AddingController
            // We cannot add FluentValidation

            services.AddControllers()
                    .AddFluentValidation(ops =>
                    {
                        ops.RegisterValidatorsFromAssemblyContaining<IUserService>();
                        ops.ImplicitlyValidateChildProperties = false;
                        ops.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                        ops.LocalizationEnabled = false;
                    });
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
