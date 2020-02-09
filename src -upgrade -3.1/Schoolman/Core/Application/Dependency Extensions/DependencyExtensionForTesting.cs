using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;


namespace Application.Extensions
{
    public static partial class DependencyExtension
    {
        /// <summary>
        /// This extenstion method is indentended for Integration testing
        /// Yet we just add the same service as in above extension
        /// BUT
        /// Things can change, so you had better to use this extension rather that above one
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationLayerForTesting(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSeq());
            services.AddMediatR(typeof(DependencyExtension).Assembly);
            services.AddAutoMapper(typeof(DependencyExtension));
            services.AddHttpContextAccessor();

        }
    }
}