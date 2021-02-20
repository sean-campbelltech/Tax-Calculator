using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Campbelltech.TaxCalculation.Domain.Calculations;
using Campbelltech.TaxCalculation.Domain.Configuration;
using Campbelltech.TaxCalculation.Domain.Repositories;
using Campbelltech.TaxCalculation.Domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Campbelltech.TaxCalculation.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime and used to add services to the container.
        /// </summary>
        /// <param name="services">IServiceCollection that secifies the contract for a collection of service descriptors.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // service registration for DI
            services.Configure<Config>(Configuration);
            services.AddScoped<IProgressiveTaxRateRepository, ProgressiveTaxRateRepository>();
            var taxCalculation = new TaxCalculationFactory().Create(services);
            services.AddTransient<ITaxCalculation>(x => taxCalculation);
            services.AddScoped<IPostalCodeTaxRepository, PostalCodeTaxRepository>();
            services.AddScoped<ITaxCalculationRepository, TaxCalculationRepository>();
            services.AddScoped<ITaxCalculationService, TaxCalculationService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Campbelltech.TaxCalculation.Api", Version = "v1" });

                // set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// Used to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">Defines a class that provides the mechanisms to configure an application's request pipeline.</param>
        /// <param name="env">Provides information about the web hosting environment an application is running in.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Campbelltech.TaxCalculation.Api v1"));
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
