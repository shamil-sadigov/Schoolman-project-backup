using Application.Services;
using Common.IntegrationTest.Factories;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Common.IntegrationTest
{
    public class BusinessLayerTest : BasicTest
    {
        public BusinessLayerTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) { }



    }
}
