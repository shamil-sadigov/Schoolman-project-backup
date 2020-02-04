﻿using Application.Services;
using AutoMapper;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace Application.Extensions
{
    public static class DependencyExtension
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyExtension).Assembly);
            services.AddAutoMapper(typeof(DependencyExtension));
        }
    }
}