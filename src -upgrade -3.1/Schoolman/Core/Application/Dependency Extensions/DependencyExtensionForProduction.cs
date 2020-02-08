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
        /// This extenstion method is indentended for Preasentation.Web layer
        /// </summary>
        /// <param name="services"></param>
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder => loggingBuilder.AddSeq());
            services.AddMediatR(typeof(DependencyExtension).Assembly);
            services.AddAutoMapper(typeof(DependencyExtension));
        }
    }
}