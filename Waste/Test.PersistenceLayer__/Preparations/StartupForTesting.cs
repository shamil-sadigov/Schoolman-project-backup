using Application.Extensions;
using Authentication;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistence;
using Schoolman.Student.Core.Application.Interfaces;

namespace Test.Shared.Preparations
{
    /// <summary>
    /// In this test project we test DbContext and Repositories
    /// </summary>
    public class StartupForTesting
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddPersistenceLayerForTesting();
        }

    }
}
