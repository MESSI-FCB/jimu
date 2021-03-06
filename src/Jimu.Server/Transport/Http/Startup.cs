﻿using Jimu.Logger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Jimu.Server.Transport.Http
{
    public class Startup : IStartup
    {
        readonly Stack<Func<RequestDel, RequestDel>> _middlewares;
        private readonly IServiceEntryContainer _serviceEntryContainer;
        private readonly ILogger _logger;
        public Startup(IConfiguration configuration, Stack<Func<RequestDel, RequestDel>> middlewares, IServiceEntryContainer serviceEntryContainer, ILogger logger)
        {
            Configuration = configuration;
            _middlewares = middlewares;
            _serviceEntryContainer = serviceEntryContainer;
            _logger = logger;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(Microsoft.AspNetCore.Builder.IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseMiddleware<HttpMiddleware>(_middlewares, _serviceEntryContainer, _logger);
        }

    }
}
